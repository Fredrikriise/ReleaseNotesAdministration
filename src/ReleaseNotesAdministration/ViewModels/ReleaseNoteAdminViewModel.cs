using System;
using System.ComponentModel.DataAnnotations;

namespace ReleaseNotesAdministration.ViewModels
{
    public class ReleaseNoteAdminViewModel
    {
        public string Title { get; set; }

        public string BodyText { get; set; }

        [Key]
        public int? Id { get; set; }

        public int ProductId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime? LastUpdateDate { get; set; }
    }
}
