using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotes.Models;
using ReleaseNotes.ViewModels;

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


                foreach(var releaseNote in releaseNotes.ReleaseNotes)
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
                        LastUpdatedDate = releaseNote.LasteUpdatedDate
                    };

                    list.Add(releaseNoteVm);

                    // For debug
                    Console.WriteLine(releaseNote);
                }
            } else
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



                var TargetProductId = 1;

                List<ReleaseNoteViewModel> releaseNotesListNew = new List<ReleaseNoteViewModel>();

                for (var i = 0; i < releaseNotesListNew.Count; i++)
                {
                    if (releaseNotesListNew[i].ProductId == TargetProductId)
                    {
                        releaseNotesListNew.Add(new ReleaseNoteViewModel
                        {
                            Title = releaseNotesListNew[i].Title,
                            Bodytext = releaseNotesListNew[i].Bodytext,
                            Id = releaseNotesListNew[i].Id,
                            ProductId = releaseNotesListNew[i].ProductId,
                            CreatedBy = releaseNotesListNew[i].CreatedBy,
                            CreatedDate = releaseNotesListNew[i].CreatedDate,
                            LastUpdatedBy = releaseNotesListNew[i].LastUpdatedBy,
                            LastUpdatedDate = releaseNotesListNew[i].LastUpdatedDate
                        });

                    }
                    ViewData.Model = releaseNotesListNew;
                }
            }
            else
            {
                //Error melding
            }

            return View();
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

                List<ReleaseNoteViewModel> releaseNotesListNew = new List<ReleaseNoteViewModel>();

                for (var i = 0; i < releaseNotesListNew.Count; i++)
                {
                    if (releaseNotesListNew[i].ProductId == TargetProductId)
                    {
                        releaseNotesListNew.Add(new ReleaseNoteViewModel
                        {
                            Title = releaseNotesListNew[i].Title,
                            Bodytext = releaseNotesListNew[i].Bodytext,
                            Id = releaseNotesListNew[i].Id,
                            ProductId = releaseNotesListNew[i].ProductId,
                            CreatedBy = releaseNotesListNew[i].CreatedBy,
                            CreatedDate = releaseNotesListNew[i].CreatedDate,
                            LastUpdatedBy = releaseNotesListNew[i].LastUpdatedBy,
                            LastUpdatedDate = releaseNotesListNew[i].LastUpdatedDate
                        });
                    }
                    ViewData.Model = releaseNotesListNew;
                }
            }
            else
            {
                //Error melding
            }

            return View(list);
        }

        public async Task<IActionResult> TalentOnboarding()
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync("/ReleaseNotes/");
            var list = new List<ReleaseNoteViewModel>();

            if (releaseNotesResult.IsSuccessStatusCode)
            {

                var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
                var releaseNotes = JsonConvert.DeserializeObject<ReleaseNoteList>(responseStream);

                var TargetProductId = 1;

                List<ReleaseNoteViewModel> releaseNotesListNew = new List<ReleaseNoteViewModel>();

                for (var i = 0; i < releaseNotesListNew.Count; i++)
                {
                    if (releaseNotesListNew[i].ProductId == TargetProductId)
                    {
                        releaseNotesListNew.Add(new ReleaseNoteViewModel
                        {
                            Title = releaseNotesListNew[i].Title,
                            Bodytext = releaseNotesListNew[i].Bodytext,
                            Id = releaseNotesListNew[i].Id,
                            ProductId = releaseNotesListNew[i].ProductId,
                            CreatedBy = releaseNotesListNew[i].CreatedBy,
                            CreatedDate = releaseNotesListNew[i].CreatedDate,
                            LastUpdatedBy = releaseNotesListNew[i].LastUpdatedBy,
                            LastUpdatedDate = releaseNotesListNew[i].LastUpdatedDate
                        });
                    }
                    ViewData.Model = releaseNotesListNew;
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