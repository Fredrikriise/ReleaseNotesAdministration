using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReleaseNotesAdministration.Models;
using System.Diagnostics;
using System.Net.Http;

namespace ReleaseNotesAdministration.Controllers
{
    public class ReleaseNotesAdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _releaseNotesClient;

        public ReleaseNotesAdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _releaseNotesClient = _httpClientFactory.CreateClient("ReleaseNotesApiClient");
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
