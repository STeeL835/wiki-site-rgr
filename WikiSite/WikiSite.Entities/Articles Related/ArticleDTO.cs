using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiSite.Entities
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string ShortUrl { get; set; }
        public Guid AuthorId { get; set; }
        public string Heading { get; set; }
        public DateTime CreationDate { get; set; }
    }
}