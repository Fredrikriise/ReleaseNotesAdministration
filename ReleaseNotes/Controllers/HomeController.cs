using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReleaseNotes.Models;

namespace ReleaseNotes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LatestRelease()
        {
            return View();
        }

        public IActionResult ListLatestReleaseNote()
        {
            var bodytextData = "Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis id duis, nisl nulla risus.";

            List<releaseNotes> releaseNotesList = new List<releaseNotes>
            {
                new releaseNotes {
                    title = "Release note 0.1 - Onboarding",
                    bodytext = bodytextData,
                    id = 1,
                    productId = 1,
                    createdBy = "Fredrik Svevad Riise",
                    createdDate = DateTime.ParseExact("27/01/2020", "dd/MM/yyyy", null),
                    lastUpdatedBy = "",
                    lastUpdatedDate = null,
                },

                new releaseNotes {
                    title = "Release note 0.93 - Manager",
                    bodytext = bodytextData,
                    id = 2,
                    productId = 2,
                    createdBy = "Felix Thu Falkendal Nilsen",
                    createdDate = DateTime.ParseExact("28/01/2020", "dd/MM/yyyy", null),
                    lastUpdatedBy = "",
                    lastUpdatedDate = null,
                }
            };

            for(var i = 0; i < releaseNotesList.Count; i++)
            {
                var val1 = 0;
                if(releaseNotesList[i].id > val1 || releaseNotesList[i].id == val1)
                {
                    val1 = releaseNotesList[i].id;
                }
                //Currently it gets the release note that was last put in the mock data/data base, maybe change it to the newest date?
                foreach (var item in releaseNotesList.Where(n => n.id == val1))
                {
                    List<releaseNotes> releaseNotesListNew = new List<releaseNotes>
                        {
                            new releaseNotes {
                                title = item.title,
                                bodytext = item.bodytext,
                                id = item.id,
                                productId = item.productId,
                                createdBy = item.createdBy,
                                createdDate = item.createdDate,
                                lastUpdatedBy = item.lastUpdatedBy,
                                lastUpdatedDate = item.lastUpdatedDate,
                            }
                        };
                    ViewData.Model = releaseNotesListNew;
                }
            }
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

  
        public ActionResult ListReleaseNotes() 
        {
            var bodytextData = "Lorem ipsum dolor sit amet, in nonummy lectus venenatis posuere risus ipsum, nulla vel lorem vitae bibendum sed, elit lacinia urna convallis eget placerat, duis wisi mauris nullam mauris, nulla vitae eu nunc nisl est.Odio justo dui ut nulla proin turpis, facere varius dolor eu ipsum congue orci, dolor lorem facilisis mauris euismod, viverra ipsum eros conubia tellus habitant. Mauris fusce egestas sodales rutrum, tellus odio tortor donec justo nec, aptent dictum dui elit mi dui, diam aliquam suscipit placerat, justo turpis integer sed.Leo ac eros ullamcorper eum sapien quam, ut quis felis, magna senectus fringilla eu ultricies vel, ac arcu sodales at urna sit mattis, nulla imperdiet quisque pede sit rutrum.Suscipit suspendisse. In hendrerit ipsum pellentesque aptent sollicitudin sapien, donec magna in cras in pulvinar quisque, eros adipiscing dui cursus hendrerit. Diam quam. Nunc elit elit semper in, nulla nam eros nonummy vestibulum suscipit, sed vitae. Vulputate ac sagittis amet nulla, ipsum aenean ante quis id duis, nisl nulla risus.";

            List<releaseNotes> releaseNotesList = new List<releaseNotes>
            {
                new releaseNotes { 
                    title = "Release note 0.1 - Onboarding",
                    bodytext = bodytextData,
                    id = 1,
                    productId = 1,
                    createdBy = "Fredrik Svevad Riise",
                    createdDate = DateTime.ParseExact("27/01/2020", "dd/MM/yyyy", null),
                    lastUpdatedBy = "",
                    lastUpdatedDate = null,
                },
                 
                new releaseNotes {
                    title = "Release note 0.93 - Manager",
                    bodytext = bodytextData,
                    id = 2,
                    productId = 2,
                    createdBy = "Felix Thu Falkendal Nilsen",
                    createdDate = DateTime.ParseExact("28/01/2020", "dd/MM/yyyy", null),
                    lastUpdatedBy = "",
                    lastUpdatedDate = null,
                }
            };

            ViewData.Model = releaseNotesList;

            return View();
        } 
    } 
}
