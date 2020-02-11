using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Repository.Models.DataTransferObjects
{
    public class ProductDto
    {
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
    }
}
