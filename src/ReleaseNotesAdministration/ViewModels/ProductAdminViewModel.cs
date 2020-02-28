using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseNotesAdministration.ViewModels
{
    public class ProductAdminViewModel
    {
        [Key]
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required to create a new product!")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Image url is required to create a new product!")]
        public string ProductImage { get; set; }

        [Required(ErrorMessage = "Product description is required to create a new product!")]
        public string ProductDescription { get; set; }
    }
}
