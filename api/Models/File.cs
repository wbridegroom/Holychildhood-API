using System;

namespace api.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string BlobId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int FileContentId { get; set; }
    }
}
