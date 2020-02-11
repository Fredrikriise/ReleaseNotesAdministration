using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Repository.Models
{
    class ReleaseNoteDto
    {
        public string Title { get; set; }
        public string BodyText { get; set; }
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
