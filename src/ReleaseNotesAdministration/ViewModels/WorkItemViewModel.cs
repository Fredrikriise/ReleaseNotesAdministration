using System.ComponentModel.DataAnnotations;

namespace ReleaseNotesAdministration.ViewModels
{
    public class WorkItemViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required!")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Assigned to is required!")]
        public string AssignedTo { get; set; }
        [Required(ErrorMessage = "State is required!")]
        public string State { get; set; }
    }
}
