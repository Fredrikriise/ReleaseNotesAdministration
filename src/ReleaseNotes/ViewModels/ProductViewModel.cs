using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
    }
}
