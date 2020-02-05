using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReleaseNotes.Models;
using ReleaseNotes.ViewModels;

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
            List<Product> productList = new List<Product>
            {
                new Product
                {
                    productID = 1,
                    productName = "Talent Recruiter",
                    productImage = "pic-recruiter.png",
                    ProductDescription = "TalentRecruiter"
                },
                new Product
                {
                    productID = 2,
                    productName = "Talent Onboarding",
                    productImage = "pic-onboarding.png",
                    ProductDescription = "TalentOnboarding"
                },
                new Product
                {
                    productID = 3,
                    productName = "Talent Manager",
                    productImage = "pic-manager.png",
                    ProductDescription = "TalentManager"
                }
            };

            ViewData.Model = productList;

            return View();
        }

        public IActionResult TalentRecruiter()
        {
            var Connection = new DBContext();
            List<releaseNotes> InReleaseNotes = Connection.MockDataList();

            var TargetId = 1;

            for (var i = 0; i < InReleaseNotes.Count; i++)
            {
                if (InReleaseNotes[i].productId == TargetId)
                {
                    List<releaseNotes> releaseNotesListNew = new List<releaseNotes>
                    {
                        new releaseNotes {
                            title = InReleaseNotes[i].title,
                            bodytext = InReleaseNotes[i].bodytext,
                            id = InReleaseNotes[i].id,
                            productId = InReleaseNotes[i].productId,
                            createdBy = InReleaseNotes[i].createdBy,
                            createdDate = InReleaseNotes[i].createdDate,
                            lastUpdatedBy = InReleaseNotes[i].lastUpdatedBy,
                            lastUpdatedDate = InReleaseNotes[i].lastUpdatedDate,
                        }
                    };
                    ViewData.Model = releaseNotesListNew;
                }
            }
            return View();
        }

        public IActionResult TalentOnboarding()
        {

            var viewModel = new HomeControllerViewModel { ReleaseNotes = new List<ReleaseNoteViewModel>() };

            // Call API

           /* for(var i = 0; i< 10; i++)
            {
                viewModel.ReleaseNotes.Add(new ReleaseNoteViewModel { Title = "Blah" });
            } */

           var Connection = new DBContext();
           List<releaseNotes> InReleaseNotes = Connection.MockDataList();

           //var TargetId = 2;

           var talentOnboardingReleaseNotes = InReleaseNotes.Where(x => x.productId == 3).ToList();
            /*
           for (var i = 0; i < InReleaseNotes.Count; i++)
           {
               if (InReleaseNotes[i].productId == TargetId)
               {
                   List<releaseNotes> releaseNotesListNew = new List<releaseNotes>
                   {
                       new releaseNotes {
                           title = InReleaseNotes[i].title,
                           bodytext = InReleaseNotes[i].bodytext,
                           id = InReleaseNotes[i].id,
                           productId = InReleaseNotes[i].productId,
                           createdBy = InReleaseNotes[i].createdBy,
                           createdDate = InReleaseNotes[i].createdDate,
                           lastUpdatedBy = InReleaseNotes[i].lastUpdatedBy,
                           lastUpdatedDate = InReleaseNotes[i].lastUpdatedDate,
                       }
                   };
                   ViewData.Model = releaseNotesListNew;
               }
           } */
            ViewData.Model = talentOnboardingReleaseNotes;

            return View(viewModel);
        }

        public IActionResult TalentManager()
        {
            var Connection = new DBContext();
            List<releaseNotes> InReleaseNotes = Connection.MockDataList();

            var TargetId = 3;

            List<releaseNotes> releaseNotesListNew = new List<releaseNotes>();

            for (var i = 0; i < InReleaseNotes.Count; i++)
            {
                if (InReleaseNotes[i].productId == TargetId)
                {
                    releaseNotesListNew.Add( new releaseNotes
                    {
                        title = InReleaseNotes[i].title,
                        bodytext = InReleaseNotes[i].bodytext,
                        id = InReleaseNotes[i].id,
                        productId = InReleaseNotes[i].productId,
                        createdBy = InReleaseNotes[i].createdBy,
                        createdDate = InReleaseNotes[i].createdDate,
                        lastUpdatedBy = InReleaseNotes[i].lastUpdatedBy,
                        lastUpdatedDate = InReleaseNotes[i].lastUpdatedDate
                    });
                } 

                if(releaseNotesListNew.Count < 1)
                {
                    var TargetNotFound = "There is currently no release notes published for this product.";

                    releaseNotesListNew.Add(new releaseNotes
                    {
                        title = "",
                        bodytext = TargetNotFound,
                        id = null,
                        productId = null,
                        createdBy = InReleaseNotes[i].createdBy,
                        createdDate = InReleaseNotes[i].createdDate,
                        lastUpdatedBy = InReleaseNotes[i].lastUpdatedBy,
                        lastUpdatedDate = InReleaseNotes[i].lastUpdatedDate
                    });
                }

                ViewData.Model = releaseNotesListNew;
            }
            return View();
        }


        public IActionResult LatestRelease()
        {
            return View();
        }

        public IActionResult ListLatestReleaseNote()
        {
            // Get release note from API
            //var client = GetClient();
            //var response = client.SendRequest( "https://releasenotes-api.talentech.io/v1/releasenotes/12345678");
            // r = response.DeserializeJson<ReleaseNotes>();
            var r = new releaseNotes
            {
                title = "Test"
            };

            var releaseNoteViweModel = new ReleaseNoteViewModel { Title = r.title };



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
                    lastUpdatedBy = "Felix Thu Falkendal Nilsen",
                    lastUpdatedDate = DateTime.ParseExact("31/01/2020", "dd/MM/yyyy", null),
                }
            };

            DateTime? val1 = DateTime.MinValue;

            for (var i = 0; i < releaseNotesList.Count; i++)
            {
                if (releaseNotesList[i].createdDate > val1 || releaseNotesList[i].createdDate == val1)
                {
                    val1 = releaseNotesList[i].createdDate;

                    List<releaseNotes> releaseNotesListNew = new List<releaseNotes>
                    {
                        new releaseNotes {
                            title = releaseNotesList[i].title,
                            bodytext = releaseNotesList[i].bodytext,
                            id = releaseNotesList[i].id,
                            productId = releaseNotesList[i].productId,
                            createdBy = releaseNotesList[i].createdBy,
                            createdDate = releaseNotesList[i].createdDate,
                            lastUpdatedBy = releaseNotesList[i].lastUpdatedBy,
                            lastUpdatedDate = releaseNotesList[i].lastUpdatedDate,
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

            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("username", "password");
            //http://csharp.net-informations.com/communications/csharp-smtp-mail.htm
            return View();
        } 
    } 
}
