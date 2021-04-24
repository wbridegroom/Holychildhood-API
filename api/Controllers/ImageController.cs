using api.ViewModels;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {

        private readonly IConfiguration configuration;

        public ImageController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // GET: api/<ImageController>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IEnumerable<ImageViewModel> List()
        {
            var blobClient = new BlobServiceClient(configuration.GetConnectionString("BlobConnection"));
            var container = blobClient.GetBlobContainerClient("images");
            var images = container.GetBlobs();
            var imageNames = new List<ImageViewModel>();
            foreach (var image in images)
            {
                imageNames.Add(new ImageViewModel { Url = "https://hccwebfiles.blob.core.windows.net/images/" + image.Name });
            }
            return imageNames;
        }

        // POST api/<ImageController>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ImageViewModel>> Upload()
        {
            var request = HttpContext.Request;

            var filesCount = request.Form.Files.Count;
            if (filesCount == 0) return NotFound();

            var file = request.Form.Files.GetFile("file");
            if (file == null) return NotFound();

            var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;

            var name = GenerateFileName(file.FileName);
            var blobClient = new BlobServiceClient(configuration.GetConnectionString("BlobConnection"));
            var container = blobClient.GetBlobContainerClient("images");
            await container.GetBlobClient(name).UploadAsync(stream, true);
            stream.Close();

            return new ImageViewModel { Link = "https://hccwebfiles.blob.core.windows.net/images/" + name };
        }

        // DELETE api/<ImageController>/5
        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete()
        {
            var request = HttpContext.Request;
            if (!request.Form.ContainsKey("src")) return NotFound();

            var src = request.Form["src"];
            var imageName = src.ToString().Substring(src.ToString().LastIndexOf("/") + 1);

            var blobClient = new BlobServiceClient(configuration.GetConnectionString("BlobConnection"));
            var container = blobClient.GetBlobContainerClient("images");
            await container.GetBlobClient(imageName).DeleteAsync();

            return NoContent();
        }

        private static string GenerateFileName(string fileName)
        {
            var extension = fileName.Substring(fileName.LastIndexOf('.') + 1);
            var sha1 = SHA1.Create();
            var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString()));
            var sb = new StringBuilder();
            foreach (var b in hashBytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb + "." + extension;
        }
    }
}
