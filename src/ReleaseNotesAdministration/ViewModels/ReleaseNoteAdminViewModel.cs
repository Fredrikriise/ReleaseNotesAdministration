using System;
using System.ComponentModel.DataAnnotations;

namespace ReleaseNotesAdministration.ViewModels
{
    public class ReleaseNoteAdminViewModel
    {
        [Required(ErrorMessage ="Title is required to create a new release note!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Body text is required to create a new release note!")]
        public string BodyText { get; set; }

        [Key]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Product id is required to create a new release note!")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Author is required to create a new release note!")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "Created date is required to create a new release note!")]
        public DateTime? CreatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime? LastUpdateDate { get; set; }
    }
}
