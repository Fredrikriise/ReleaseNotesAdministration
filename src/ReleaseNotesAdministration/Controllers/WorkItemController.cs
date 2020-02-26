using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReleaseNotesAdministration.ViewModels;

namespace ReleaseNotesAdministration.Controllers
{
    public class WorkItemController : Controller
    {
        public IActionResult ListWorkItems()
        {
            List<WorkItemViewModel> workItemList = new List<WorkItemViewModel>
            {
                new WorkItemViewModel
                {
                    Id = 21703,
                    Title = "Creating neccessary files and methods for mocking data for work items",
                    AssignedTo = "Fredrik Riise",
                    State = "Active"
                },
                new WorkItemViewModel
                {
                    Id = 21701,
                    Title = "Fix cards on user module front page",
                    AssignedTo = "Felix Nilsen",
                    State = "Active"
                },
                new WorkItemViewModel
                {
                    Id = 21625,
                    Title = "Adding the styling to correct file (User module)",
                    AssignedTo = "Fredrik Riise",
                    State = "New"
                },
                new WorkItemViewModel
                {
                    Id = 21680,
                    Title = "Make listing of 'All Release Notes' descending based of publish-date",
                    AssignedTo = "Fredrik Riise",
                    State = "New"
                },
                new WorkItemViewModel
                {
                    Id = 21702,
                    Title = "Make 'Loading...'-function when loading views",
                    AssignedTo = "Felix Nilsen",
                    State = "New"
                },
                new WorkItemViewModel
                {
                    Id = 21700,
                    Title = "Adding TinyMCE text editor",
                    AssignedTo = "Fredrik Riise",
                    State = "Active"
                }
            };
            ViewData.Model = workItemList;
            return View();
        }
    }
}