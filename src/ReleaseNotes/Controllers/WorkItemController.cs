using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotes.Models;
using ReleaseNotes.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReleaseNotes.Controllers
{
    public class WorkItemController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _workItemsClient;

        public WorkItemController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _workItemsClient = _httpClientFactory.CreateClient("ReleaseNotesApiClient");
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