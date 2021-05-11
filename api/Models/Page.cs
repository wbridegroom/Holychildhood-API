using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace api.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Index { get; set; }
        public int? MenuItemId { get; set; }
        [JsonIgnore]
        public MenuItem MenuItem { get; set; }
        public List<PageContent> PageContents { get; set; }
        [JsonIgnore]
        public Page Parent { get; set; }
        public List<Page> Children { get; set; }
    }
}
