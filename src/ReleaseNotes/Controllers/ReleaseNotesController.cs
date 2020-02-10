using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotes.Models;
using ReleaseNotes.ViewModels;
using System;
using System.Collections.Generic;
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

        public async Task<IActionResult> Index()
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync("/ReleaseNotes/");
            var list = new List<ReleaseNoteViewModel>();

            if (releaseNotesResult.IsSuccessStatusCode)
            {

                var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
                var releaseNotes = JsonConvert.DeserializeObject<ReleaseNoteList>(responseStream);


                foreach (var releaseNote in releaseNotes.ReleaseNotes)
                {
                    var releaseNoteVm = new ReleaseNoteViewModel()
                    {
                        Title = releaseNote.Title,
                        Bodytext = releaseNote.BodyText,
                        Id = releaseNote.Id,
                        ProductId = releaseNote.ProductId,
                        CreatedBy = releaseNote.CreatedBy,
                        CreatedDate = releaseNote.CreatedDate,
                        LastUpdatedBy = releaseNote.LastUpdatedBy,
                        LastUpdatedDate = releaseNote.LastUpdatedDate
                    };

                    list.Add(releaseNoteVm);

                    // For debug
                    Console.WriteLine(releaseNote);
                }
            }
            else
            {
                //Error melding
            }

            return View(list);
        }

        public async Task<IActionResult> TalentManager()
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync("/ReleaseNotes/");
            var list = new List<ReleaseNoteViewModel>();

            if (releaseNotesResult.IsSuccessStatusCode)
            {

                var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
                var releaseNotes = JsonConvert.DeserializeObject<ReleaseNoteList>(responseStream);


                var TargetProductId = 3;

                foreach (var releaseNote in releaseNotes.ReleaseNotes)
                {
                    if (releaseNote.ProductId == TargetProductId)
                    {
                        var releaseNoteVM = new ReleaseNoteViewModel
                        {
                            Title = releaseNote.Title,
                            Bodytext = releaseNote.BodyText,
                            Id = releaseNote.Id,
                            ProductId = releaseNote.ProductId,
                            CreatedBy = releaseNote.CreatedBy,
                            CreatedDate = releaseNote.CreatedDate,
                            LastUpdatedBy = releaseNote.LastUpdatedBy,
                            LastUpdatedDate = releaseNote.LastUpdatedDate
                        };
                        list.Add(releaseNoteVM);
                    }
                }
            }
            else
            {
                //Error melding
            }
            return View(list);
        }

        public async Task<IActionResult> TalentRecruiter()
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync("/ReleaseNotes/");
            var list = new List<ReleaseNoteViewModel>();

            if (releaseNotesResult.IsSuccessStatusCode)
            {

                var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
                var releaseNotes = JsonConvert.DeserializeObject<ReleaseNoteList>(responseStream);

                var TargetProductId = 1;

                //var talentRecruiterReleaseNotes = releaseNotes.ReleaseNotes.Where(x => x.ProductId == TargetProductId);
                
                foreach (var releaseNote in releaseNotes.ReleaseNotes)
                {
                    if (releaseNote.ProductId == TargetProductId)
                    {
                        var releaseNoteVM = new ReleaseNoteViewModel
                        {
                            Title = releaseNote.Title,
                            Bodytext = releaseNote.BodyText,
                            Id = releaseNote.Id,
                            ProductId = releaseNote.ProductId,
                            CreatedBy = releaseNote.CreatedBy,
                            CreatedDate = releaseNote.CreatedDate,
                            LastUpdatedBy = releaseNote.LastUpdatedBy,
                            LastUpdatedDate = releaseNote.LastUpdatedDate
                        };
                        list.Add(releaseNoteVM);
                    }
                } 
            }
            else
            {
                //Error melding
            }
     
            return View();
        }

        public async Task<IActionResult> TalentOnboarding()
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync("/ReleaseNotes/");
            var list = new List<ReleaseNoteViewModel>();

            if (releaseNotesResult.IsSuccessStatusCode)
            {

                var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
                var releaseNotes = JsonConvert.DeserializeObject<ReleaseNoteList>(responseStream);

                var TargetProductId = 2;

                foreach (var releaseNote in releaseNotes.ReleaseNotes)
                {
                    if (releaseNote.ProductId == TargetProductId)
                    {
                        var releaseNoteVM = new ReleaseNoteViewModel
                        {
                            Title = releaseNote.Title,
                            Bodytext = releaseNote.BodyText,
                            Id = releaseNote.Id,
                            ProductId = releaseNote.ProductId,
                            CreatedBy = releaseNote.CreatedBy,
                            CreatedDate = releaseNote.CreatedDate,
                            LastUpdatedBy = releaseNote.LastUpdatedBy,
                            LastUpdatedDate = releaseNote.LastUpdatedDate
                        };
                        list.Add(releaseNoteVM);
                    }
                }
            }
            else
            {
                //Error melding
            }
            return View(list);
        }
    }
}