using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
    }
}
