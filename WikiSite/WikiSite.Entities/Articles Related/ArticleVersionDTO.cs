using System;

namespace WikiSite.Entities
{
    public class ArticleVersionDTO
    {
        public Guid Id { get; set; }
        public Guid ArticleId { get; set; }
        public Guid ContentId { get; set; }
        public DateTime LastEditDate { get; set; }
        public Guid EditionAuthorId { get; set; }
        public bool IsApproved { get; set; }
    }
}