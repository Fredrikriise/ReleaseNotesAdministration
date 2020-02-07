using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseNotes.Models
{
    public class ReleaseNoteApiModel
    {
        public string Title { get; set; }
        public string BodyText { get; set; }
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LasteUpdatedDate { get; set; }
    }
}
