using api.Database;
using api.ViewModels;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly AppDbContext dbContext;
        private readonly IConfiguration configuration;

        public FileController(AppDbContext dbContext, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
        }

        [HttpGet("{id}")]
        public IActionResult Download(string id)
        {
            var fileName = id + ".pdf";
            var blobClient = new BlobServiceClient(configuration.GetConnectionString("BlobConnection"));
            var container = blobClient.GetBlobContainerClient("files");
            var stream = container.GetBlobClient(fileName).OpenRead();

            if (stream == null) return NotFound();

            return new FileStreamResult(stream, "application/pdf");
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<FileViewModel>> Upload()
        {
            var request = HttpContext.Request;

            var filesCount = request.Form.Files.Count;
            if (filesCount == 0) return NotFound();

            var file = request.Form.Files.GetFile("file");
            if (file == null) return NotFound();

            var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;

            var extension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);
            var name = request.Form["name"].ToString();
            var date = request.Form["date"].ToString();
            var fileContentIdStr = request.Form["FileContentId"];

            var blobId = GenerateFileName();
            var fileName = blobId + "." + extension;
            var blobClient = new BlobServiceClient(configuration.GetConnectionString("BlobConnection"));
            var container = blobClient.GetBlobContainerClient("files");
            await container.GetBlobClient(fileName).UploadAsync(stream, true);
            stream.Close();

            var dbFile = new Models.File
            {
                Name = name,
                Extension = extension,
                BlobId = blobId,
                CreatedAt = DateTime.Parse(date),
                FileContentId = int.Parse(fileContentIdStr)
            };
            await dbContext.Files.AddAsync(dbFile);
            await dbContext.SaveChangesAsync();

            return new FileViewModel { Link = "https://hccwebfiles.blob.core.windows.net/files/" + name };
        }

        private static string GenerateFileName()
        {
            var sha1 = SHA1.Create();
            var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString()));
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }

    }
}
