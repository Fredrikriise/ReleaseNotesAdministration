using System;

namespace Api.Models
{
    public class ReleaseNotesModel
    {
        public string Title { get; set; }
        public string BodyText { get; set; }
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }
}
