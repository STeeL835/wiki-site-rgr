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
        public string ShortDefinition { get; set; }
        public string TextContent { get; set; }
        public string ThumbnailURL { get; set; }
    }
}
