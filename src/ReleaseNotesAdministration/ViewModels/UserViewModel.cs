using System.ComponentModel.DataAnnotations;

namespace ReleaseNotesAdministration.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Username is required!")]
        public string username { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        public string password { get; set; }
    }
}
