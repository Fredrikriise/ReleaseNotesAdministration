using System;
using System.ComponentModel.DataAnnotations;

namespace ReleaseNotesAdministration.ViewModels
{
    public class ReleaseNoteAdminViewModel
    {
       [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Body text is required!")]
        public string BodyText { get; set; }
        [Key]
        public int? Id { get; set; }
        [Required(ErrorMessage = "Product id is required!")]
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Author is required!")]
        public string CreatedBy { get; set; }
        [Required(ErrorMessage = "Created date is required!")]
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool IsDraft { get; set; }
        [Required(ErrorMessage = "Related work items are required!")]
        public string PickedWorkItems { get; set; }

    }
}
