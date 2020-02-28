using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotesAdministration.Models;
using ReleaseNotesAdministration.ViewModels;
using System;
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

        // Lists all release notes for all products
        public async Task<IActionResult> ListAllReleaseNotes()
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
        public async Task<IActionResult> CreateReleaseNote(ReleaseNoteAdminApiModel releaseNote)
        {
            var obj = new ReleaseNoteAdminApiModel
            {
                Title = releaseNote.Title,
                BodyText = releaseNote.BodyText,
                ProductId = releaseNote.ProductId,
                CreatedBy = releaseNote.CreatedBy,
                CreatedDate = DateTime.Now
            };

            var jsonString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            await _releaseNotesClient.PostAsync("/ReleaseNotes/", content);

            return RedirectToAction("ListReleaseNotes");
        }

        // Method for getting release note object to edit
        public async Task<IActionResult> EditReleaseNote(int Id)
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync($"/ReleaseNotes/{Id}");

            if (!releaseNotesResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/ReleaseNotes/' failed");
            }

            var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
            var releaseNote = JsonConvert.DeserializeObject<ReleaseNoteAdminApiModel>(responseStream);

            var releaseNoteViewModel = new ReleaseNoteAdminViewModel
            {
                Title = releaseNote.Title,
                BodyText = releaseNote.BodyText,
                ProductId = releaseNote.ProductId,
                CreatedBy = releaseNote.CreatedBy,
                CreatedDate = releaseNote.CreatedDate,
                LastUpdatedBy = releaseNote.LastUpdatedBy,
                LastUpdateDate = DateTime.Now
            };

            return View(releaseNoteViewModel);
        }

        // Method for posting edit on a release note object
        [HttpPost]
        public async Task<IActionResult> EditReleaseNote(int? Id, ReleaseNoteAdminViewModel releaseNote)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(releaseNote);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var transportData = await _releaseNotesClient.PutAsync($"/ReleaseNotes/{Id}", content);
                return RedirectToAction("ListReleaseNotes");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Method for getting an release note object to view
        public async Task<IActionResult> ViewReleaseNote(int Id)
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync($"/ReleaseNotes/{Id}");

            if (!releaseNotesResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/ReleaseNotes/' failed");
            }

            var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
            var releaseNote = JsonConvert.DeserializeObject<ReleaseNoteAdminViewModel>(responseStream);
            return View(releaseNote);
        }

        // Method for deleting object
        [HttpPost]
        public async Task<IActionResult> DeleteReleaseNote(int? Id)
        {
            try
            {
                var transportData = await _releaseNotesClient.DeleteAsync($"/ReleaseNotes/{Id}");
                return RedirectToAction("ListReleaseNotes");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
