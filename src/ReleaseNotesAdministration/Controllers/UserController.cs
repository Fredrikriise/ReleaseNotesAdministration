using Microsoft.AspNetCore.Mvc;

namespace ReleaseNotesAdministration.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}