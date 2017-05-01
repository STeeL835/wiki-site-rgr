using System;

namespace WikiSite.Entities
{
    public class ArticleBDO
    {
        //ArticleDTO
        public Guid Id { get; set; }
        public string ShortUrl { get; set; }
        public Guid AuthorId { get; set; }
        public string Heading { get; set; }
        public DateTime CreationDate { get; set; }

        //ArticleVersionDTO
        public DateTime LastEditDate { get; set; }
        public Guid EditionAuthorId { get; set; }
        public bool IsApproved { get; set; }

        //ArticleContentDTO
        public string Definition { get; set; }
        public string Text { get; set; }
    }
}