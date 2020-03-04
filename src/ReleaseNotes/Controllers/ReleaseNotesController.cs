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
        private HttpClient _releaseNotesClient;

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

            var releaseNotesList = releaseNotes.Select(x => new ReleaseNoteViewModel
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

        public async Task<IActionResult> ListReleaseNotesForProduct(int productId)
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync($"/ReleaseNotes?productId={productId}");

            if (!releaseNotesResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/ReleaseNotes/' failed");
            }

            var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
            var releaseNotes = JsonConvert.DeserializeObject<List<ReleaseNoteApiModel>>(responseStream);


            var talentReleaseNotes = releaseNotes.Where(x => x.ProductId == productId).Select(x => new ReleaseNoteViewModel
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

            return View(talentReleaseNotes);
        }

        /*
        // Har denne her, i tilfelle vi trenger noen lignende metode senere. Feks til filtrerning
        public IActionResult ListLatestReleaseNote()
        {
            var BodytextData = "Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis id duis, nisl nulla risus.";

            List<ReleaseNoteViewModel> releaseNotesList = new List<ReleaseNoteViewModel>
            {
                new ReleaseNoteViewModel {
                    Title = "Release note 0.1 - Onboarding",
                    Bodytext = BodytextData,
                    Id = 1,
                    ProductId = 1,
                    CreatedBy = "Fredrik Svevad Riise",
                    CreatedDate = DateTime.ParseExact("27/01/2020", "dd/MM/yyyy", null),
                    LastUpdatedBy = "",
                    LastUpdatedDate = null,
                },

                new ReleaseNoteViewModel {
                    Title = "Release note 0.93 - Manager",
                    Bodytext = BodytextData,
                    Id = 2,
                    ProductId = 2,
                    CreatedBy = "Felix Thu Falkendal Nilsen",
                    CreatedDate = DateTime.ParseExact("28/01/2020", "dd/MM/yyyy", null),
                    LastUpdatedBy = "Felix Thu Falkendal Nilsen",
                    LastUpdatedDate = DateTime.ParseExact("31/01/2020", "dd/MM/yyyy", null),
                }
            };

            DateTime? val1 = DateTime.MinValue;

            for (var i = 0; i < releaseNotesList.Count; i++)
            {
                if (releaseNotesList[i].CreatedDate > val1 || releaseNotesList[i].CreatedDate == val1)
                {
                    val1 = releaseNotesList[i].CreatedDate;

                    List<ReleaseNoteViewModel> releaseNotesListNew = new List<ReleaseNoteViewModel>
                    {
                        new ReleaseNoteViewModel {
                            Title = releaseNotesList[i].Title,
                            Bodytext = releaseNotesList[i].Bodytext,
                            Id = releaseNotesList[i].Id,
                            ProductId = releaseNotesList[i].ProductId,
                            CreatedBy = releaseNotesList[i].CreatedBy,
                            CreatedDate = releaseNotesList[i].CreatedDate,
                            LastUpdatedBy = releaseNotesList[i].LastUpdatedBy,
                            LastUpdatedDate = releaseNotesList[i].LastUpdatedDate,
                        }
                    };
                    ViewData.Model = releaseNotesListNew;
                }
            }
            return View();
        }
        */

        // Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}