using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiSite.Entities
{
    public class ImageDTO
    {
        public Guid Id { get; set; }
        public byte[] Data { get; set; }
        public string Type { get; set; }
    }
}
