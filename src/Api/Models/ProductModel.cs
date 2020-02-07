using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    public class ProductModel
    {
        public int? ProductId { get; set; }
        public String ProductName { get; set; }
        public String ProductImage { get; set; }
        public String ProductDescription { get; set; }
    }
}
