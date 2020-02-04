using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotes.Models;

namespace ReleaseNotes.Controllers
{
    public class ReleaseNotesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _releaseNotesClient;

        public ReleaseNotesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

           _releaseNotesClient = _httpClientFactory.CreateClient("ReleaseNotesApiClient");
        }

        public async Task<IActionResult> Index()
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync("/ReleaseNotes/");

            if(releaseNotesResult.IsSuccessStatusCode)
            {
                using var responseStream = await releaseNotesResult.Content.ReadAsStreamAsync();
                var releaseNotes = await JsonSerializer.DeserializeAsync
                    <IEnumerable<ReleaseNoteApiModel>>(responseStream);
            } else
            {

            }

            return View();
        }
    }
}