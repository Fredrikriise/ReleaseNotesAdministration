using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseNotesAdministration.ViewModels
{
    public class ReleaseNoteAdminViewModel
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
