using System;
using System.ComponentModel.DataAnnotations;

namespace ReleaseNotes.ViewModels
{
    public class ReleaseNoteViewModel
    {
        public string Title { get; set; }

        public string Bodytext { get; set; }

        [Key]
        public int? Id { get; set; }

        public int? ProductId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTime? LastUpdateDate { get; set; }
    }
}
