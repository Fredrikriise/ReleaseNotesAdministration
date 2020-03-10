using System.ComponentModel.DataAnnotations;

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
    }
}
