using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotesAdministration.Models;
using ReleaseNotesAdministration.ViewModels;

namespace ReleaseNotesAdministration.Controllers
{
    public class WorkItemController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _workItemsClient;

        public WorkItemController(IHttpClientFactory httpClientFactory)
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

        public async Task<IActionResult> ListWorkItem(int Id)
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
    }
}