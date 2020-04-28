using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotesAdministration.Models;
using ReleaseNotesAdministration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReleaseNotesAdministration.Controllers
{
    public class WorkItemAdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _workItemsClient;

        public WorkItemAdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _workItemsClient = _httpClientFactory.CreateClient("ReleaseNotesAdminApiClient");
        }

        // Lists all work items
        public async Task<IActionResult> ListAllWorkItems()
        {
            var workItemResult = await _workItemsClient.GetAsync("/WorkItem/");

            if (!workItemResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/WorkItem/' failed");
            }

            var responseStream = await workItemResult.Content.ReadAsStringAsync();
            var workItems = JsonConvert.DeserializeObject<List<WorkItemApiModel>>(responseStream);

            var workItemList = workItems.Select(x => new WorkItemViewModel
            {
                Id = x.Id,
                Title = x.Title,
                AssignedTo = x.AssignedTo,
                State = x.State
            }).ToList();

            return View(workItemList);
        }

        public async Task<IActionResult> ViewWorkItem(int Id)
        {
            var workItemResult = await _workItemsClient.GetAsync($"/WorkItem/{Id}");

            if (!workItemResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/WorkItem/' failed");
            }

            var responseStream = await workItemResult.Content.ReadAsStringAsync();
            var workItem = JsonConvert.DeserializeObject<WorkItemApiModel>(responseStream);

            var workItemViewModel = new WorkItemViewModel
            {
                Id = workItem.Id,
                Title = workItem.Title,
                AssignedTo = workItem.AssignedTo,
                State = workItem.State
            };

            return View(workItemViewModel);
        }

        // Method for loading create-view
        public ActionResult Create()
        {
            return View();
        }

        // Method for creating product
        public async Task<IActionResult> CreateWorkItem(WorkItemApiModel workItem)
        {
            string workItemIdPattern = @"^[0-9]{1,99}$";
            var workitemIdMatch = Regex.Match((workItem.Id).ToString(), workItemIdPattern, RegexOptions.IgnoreCase);
            if (!workitemIdMatch.Success)
            {
                ModelState.AddModelError("Id", "Id may only consists of numbers!");
            }

            string workItemTitlePattern = @"^[A-Za-z0-9\s\-_,\.;:!()+']{3,99}$";
            var workitemTitleMatch = Regex.Match(workItem.Title, workItemTitlePattern, RegexOptions.IgnoreCase);
            if (!workitemTitleMatch.Success)
            {
                ModelState.AddModelError("Title", "Title must be between three and 99 characters!");
            }

            string workItemAssignedToPattern = @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$";
            var workItemAssignedToMatch = Regex.Match(workItem.AssignedTo, workItemAssignedToPattern, RegexOptions.IgnoreCase);
            if (!workItemAssignedToMatch.Success)
            {
                ModelState.AddModelError("AssignedTo", "Assigned to may only consist of characters!");
            }

            if (!ModelState.IsValid)
            {
                TempData["CreateWorkItem"] = "Failed";
                return View("Create");
            }

            var obj = new WorkItemApiModel
            {
                Id = workItem.Id,
                Title = workItem.Title,
                AssignedTo = workItem.AssignedTo,
                State = workItem.State
            };

            var jsonString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await _workItemsClient.PostAsync("/WorkItem/", content);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Failed creating work item");
            }

            TempData["CreateWorkItem"] = "Success";
            return RedirectToAction("ListAllWorkItems");
        }

        // Method for getting work item object to edit view
        public async Task<IActionResult> EditWorkItem(int Id)
        {
            var workItemResult = await _workItemsClient.GetAsync($"/WorkItem/{Id}");

            if (!workItemResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/WorkItem/' failed");
            }

            var responseStream = await workItemResult.Content.ReadAsStringAsync();
            var workItem = JsonConvert.DeserializeObject<WorkItemApiModel>(responseStream);

            var workItemViewModel = new WorkItemViewModel
            {
                Id = workItem.Id,
                Title = workItem.Title,
                AssignedTo = workItem.AssignedTo,
                State = workItem.State
            };

            return View(workItemViewModel);
        }

        // Method for posting edit on a work item object
        [HttpPost]
        public async Task<IActionResult> EditWorkItem(int Id, WorkItemViewModel workItem)
        {
            var jsonString = JsonConvert.SerializeObject(workItem);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var transportData = await _workItemsClient.PutAsync($"/WorkItem/{Id}", content);

            if(!transportData.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Could edit Work Item with id = {Id}");
            }

            string workItemIdPattern = @"^[0-9]{1,99}$";
            var workitemIdMatch = Regex.Match((workItem.Id).ToString(), workItemIdPattern, RegexOptions.IgnoreCase);
            if (!workitemIdMatch.Success)
            {
                ModelState.AddModelError("Id", "Id may only consists of numbers!");
            }

            string workItemTitlePattern = @"^[A-Za-z0-9\s\-_,\.;:!()+']{3,99}$";
            var workitemTitleMatch = Regex.Match(workItem.Title, workItemTitlePattern, RegexOptions.IgnoreCase);
            if (!workitemTitleMatch.Success)
            {
                ModelState.AddModelError("Title", "Title must be between three and 99 characters!");
            }

            string workItemAssignedToPattern = @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$";
            var workItemAssignedToMatch = Regex.Match(workItem.AssignedTo, workItemAssignedToPattern, RegexOptions.IgnoreCase);
            if (!workItemAssignedToMatch.Success)
            {
                ModelState.AddModelError("AssignedTo", "Assigned to may only consist of characters!");
            }

            if (!ModelState.IsValid)
            {
                TempData["EditWorkItem"] = "Failed";
                return View("EditWorkItem");
            }

            TempData["EditWorkItem"] = "Success";
            return RedirectToAction("ViewWorkItem", new { id = Id });
        }

        // Method for deleting object
        [HttpPost]
        public async Task<IActionResult> DeleteWorkItem(int Id)
        {
            var transportData = await _workItemsClient.DeleteAsync($"/WorkItem/{Id}");

            if(!transportData.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Couldnt delete work item with id = {Id}");
            }

            TempData["DeleteWorkitem"] = "Success";
            return RedirectToAction("ListAllWorkItems");
        }
    }
}