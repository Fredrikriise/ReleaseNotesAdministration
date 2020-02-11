using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Logic.Models
{
    public class Product
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
    }
}
