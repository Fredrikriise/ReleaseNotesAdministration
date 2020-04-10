using System.ComponentModel.DataAnnotations;

namespace ReleaseNotesAdministration.ViewModels
{
    public class ProductAdminViewModel
    {
        [Key]
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required!")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Image url is required!")]
        public string ProductImage { get; set; }
    }
}
