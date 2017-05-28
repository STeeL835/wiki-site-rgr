using System;

namespace WikiSite.Entities
{
    public class ArticleDTO
    {
        public Guid Id { get; set; }
        public string ShortUrl { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}