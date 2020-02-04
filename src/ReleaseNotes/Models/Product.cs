using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseNotes.Models
{
    public class Product
    {
        [Key]
        public int productID { get; set; }
        public string productName { get; set; }
        public string productImage { get; set; }
    }
}
