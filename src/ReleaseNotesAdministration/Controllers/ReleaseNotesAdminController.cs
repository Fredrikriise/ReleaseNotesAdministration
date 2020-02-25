using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotesAdministration.Models;
using ReleaseNotesAdministration.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseNotesAdministration.Controllers
{
    public class ReleaseNotesAdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _releaseNotesClient;

        public ReleaseNotesAdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _releaseNotesClient = _httpClientFactory.CreateClient("ReleaseNotesAdminApiClient");
        }

        // Loading all release notes for all products
        public async Task<IActionResult> ListReleaseNotes()
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync("/ReleaseNotes/");

            if (!releaseNotesResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/ReleaseNotes/' failed");
            }

            var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
            var releaseNotes = JsonConvert.DeserializeObject<List<ReleaseNoteAdminApiModel>>(responseStream);

            var releaseNotesList = releaseNotes.Select(x => new ReleaseNoteAdminViewModel
            {
                Title = x.Title,
                BodyText = x.BodyText,
                Id = x.Id,
                ProductId = x.ProductId,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastUpdatedBy = x.LastUpdatedBy,
                LastUpdateDate = x.LastUpdateDate
            }).ToList();

            return View(releaseNotesList);
        }

        // Method for loading create-view
        public ActionResult Create()
        {
            return View();
        }

        // Method for creating release note
        [HttpPost]
        public async Task<IActionResult> CreateReleaseNote(ReleaseNoteAdminApiModel releaseNote)
        {
            var test = new ReleaseNoteAdminApiModel
            {
                Title = releaseNote.Title,
                BodyText = releaseNote.BodyText,
                Id = releaseNote.Id,
                ProductId = releaseNote.ProductId,
                CreatedBy = releaseNote.CreatedBy,
                CreatedDate = releaseNote.CreatedDate,
                LastUpdatedBy = releaseNote.LastUpdatedBy,
                LastUpdateDate = releaseNote.LastUpdateDate
            };

            var jsonString = JsonConvert.SerializeObject(test);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            await _releaseNotesClient.PostAsync("/ReleaseNotes/", content);

            return RedirectToAction("ListReleaseNotes");
        }

        public async Task<IActionResult> ListWorkItems()
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
