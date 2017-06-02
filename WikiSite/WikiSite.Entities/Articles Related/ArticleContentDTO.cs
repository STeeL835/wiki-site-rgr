using System;

namespace WikiSite.Entities
{
    public class ArticleContentDTO
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string Definition { get; set; }
        public string Text { get; set; }
        public Guid ImageId { get; set; }
    }
}