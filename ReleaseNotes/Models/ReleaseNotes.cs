using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseNotes.Models
{
    public class ReleaseNotes
    {
        [Display(Name = "Title")]
        public string title { get; set; }
        public string bodytext { get; set; }
        [Key]
        public int id { get; set; }
        public int productId { get; set; }
        [Display(Name = "Creator")]
        public string createdBy { get; set; }
        [Display(Name = "Date published")]
        public DateTime? createdDate { get; set; }
        [Display(Name = "Last updated by")]
        public string lastUpdatedBy { get; set; }
        [Display(Name = "Last updated")]
        public DateTime? lastUpdatedDate { get; set; }

    }
}
