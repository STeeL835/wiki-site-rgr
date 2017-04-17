using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiSite.Entities
{
    public class ArticleContentDTO
    {
        public Guid Id { get; set; }
        public string Definition { get; set; }
        public string Text { get; set; }
    }
}