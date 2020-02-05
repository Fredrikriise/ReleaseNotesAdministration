using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseNotes.Models
{
    public class releaseNotes
    {
        [Display(Name = "Title")]
        public string title { get; set; }
        public string bodytext { get; set; }
        [Key]
        public int? id { get; set; }
        public int? productId { get; set; }
        [Display(Name = "Posted by")]
        public string createdBy { get; set; }
        [Display(Name = "Date published")]
        //Formats the datetime to only show day, month and year
        //ApplyFormatInEditMode is used when entering a date, or picking a date, so unless we will use either of those features then we can remove "ApplyFormatInEditMode"
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? createdDate { get; set; }
        [Display(Name = "Last updated by")]
        public string lastUpdatedBy { get; set; }
        [Display(Name = "Last updated")]
        //Formats the datetime to only show day, month and year
        //ApplyFormatInEditMode is used when entering a date, or picking a date, so unless we will use either of those features then we can remove "ApplyFormatInEditMode"
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? lastUpdatedDate { get; set; }

    }
}
