using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Repository.Models.DatabaseModels
{
    public class ReleaseNote
    {
        public string Title { get; set; }
        public string BodyText { get; set; }
        public int? Id { get; set; }
        public int ProductId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        public void AddReleaseNoteId(int? id)
        {
            Id = id;
        }
    }
}
