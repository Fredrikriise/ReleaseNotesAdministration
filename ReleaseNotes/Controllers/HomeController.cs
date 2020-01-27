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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

  
        public ActionResult ListReleaseNotes() 
        {
            List<releaseNotes> releaseNotesList = new List<releaseNotes>
            {
                new releaseNotes { 
                    title = "Release note 0.1 - Onboarding",
                    bodytext = "Updated submit button to have round corners",
                    id = 1,
                    productId = 1,
                    createdBy = "Fredrik Svevad Riise",
                    createdDate = DateTime.ParseExact("27/01/2020", "dd/MM/yyyy", null),
                    lastUpdatedBy = "",
                    lastUpdatedDate = DateTime.ParseExact("20/02/2002", "dd/MM/yyyy", null),
                }
            };

            ViewData.Model = releaseNotesList;

            return View();
        } 
    } 
}
