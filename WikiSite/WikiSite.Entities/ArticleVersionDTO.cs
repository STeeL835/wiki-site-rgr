using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiSite.Entities
{
    public class ArticleVersionDTO
    {
        public Guid Id { get; set; }
        public Guid ContentId { get; set; }
        public DateTime LastEditDate { get; set; }
        public Guid AuthorEditId { get; set; }
        public bool IsApproved { get; set; }
    }
}
