using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotes.Models;
using ReleaseNotes.ViewModels;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReleaseNotes.Controllers
{
    public class ReleaseNotesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _releaseNotesClient;

        public ReleaseNotesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _releaseNotesClient = _httpClientFactory.CreateClient("ReleaseNotesApiClient");
        }

        // Loading all release notes for all products
        public async Task<IActionResult> ListAllReleaseNotes()
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync("/ReleaseNotes/");

            if (!releaseNotesResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/ReleaseNotes/' failed");
            }

            var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
            var releaseNotes = JsonConvert.DeserializeObject<List<ReleaseNoteApiModel>>(responseStream);

            var releaseNotesList = releaseNotes.Where(x => x.IsDraft == false).Select(x => new ReleaseNoteViewModel
            {
                Title = x.Title,
                BodyText = x.BodyText,
                Id = x.Id,
                ProductId = x.ProductId,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastUpdatedBy = x.LastUpdatedBy,
                LastUpdateDate = x.LastUpdateDate,
                PickedWorkItems = x.PickedWorkItems
            });

            var orderedReleaseNotes = releaseNotesList.OrderByDescending(x => x.CreatedDate).ToList();

            return View(orderedReleaseNotes);
        }

        public async Task<IActionResult> ListReleaseNotesForProduct(int productId)
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync($"/ReleaseNotes?productId={productId}");

            if (!releaseNotesResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/ReleaseNotes/' failed");
            }

            var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
            var releaseNotes = JsonConvert.DeserializeObject<List<ReleaseNoteApiModel>>(responseStream);

            var releaseNotesList = releaseNotes.Where(x => x.ProductId == productId).Select(x => new ReleaseNoteViewModel
            {
                Title = x.Title,
                BodyText = x.BodyText,
                Id = x.Id,
                ProductId = x.ProductId,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastUpdatedBy = x.LastUpdatedBy,
                LastUpdateDate = x.LastUpdateDate,
                PickedWorkItems = x.PickedWorkItems
            });

            var orderedReleaseNotes = releaseNotesList.OrderByDescending(x => x.CreatedDate).ToList();

            return View(orderedReleaseNotes);
        }

        // Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}