using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReleaseNotesAdministration.Models;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using ReleaseNotesAdministration.ViewModels;
using Services.Repository.Interfaces;
using System.Text;
using System;

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
                Bodytext = x.BodyText,
                Id = x.Id,
                ProductId = x.ProductId,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastUpdatedBy = x.LastUpdatedBy,
                LastUpdateDate = x.LastUpdateDate
            }).ToList();

            return View(releaseNotesList);
        }

        public ActionResult Create()
        {
            return View();
        }

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

        /*
        public async Task<IActionResult> EditReleaseNote(int? Id)
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync("/ReleaseNotes/");

            var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
            var releaseNotes = JsonConvert.DeserializeObject<List<ReleaseNoteAdminApiModel>>(responseStream);

            var releaseNote = releaseNotes.Where(x => x.Id == Id).Select(x => new ReleaseNoteAdminViewModel
            {
                Title = x.Title,
                Bodytext = x.BodyText,
                Id = x.Id,
                ProductId = x.ProductId,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastUpdatedBy = x.LastUpdatedBy,
                LastUpdateDate = x.LastUpdateDate
            }).ToList();

            return View();
        }

        public async Task<IActionResult> sEditReleaseNote(int? Id, ReleaseNoteAdminViewModel releaseNote)
        {
            var jsonString = JsonConvert.SerializeObject(releaseNote);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            await _releaseNotesClient.PutAsync("/ReleaseNotes/", content);

            return RedirectToAction("ListReleaseNotes");
        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
