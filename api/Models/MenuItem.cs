using System.Collections.Generic;

namespace api.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Page> Pages { get; set; }
    }
}
