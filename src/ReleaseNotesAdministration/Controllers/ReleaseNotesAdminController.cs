using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotesAdministration.Models;
using ReleaseNotesAdministration.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace ReleaseNotesAdministration.Controllers
{
    public class ReleaseNotesAdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _releaseNotesClient;

        public ReleaseNotesAdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _releaseNotesClient = _httpClientFactory.CreateClient("ReleaseNotesAdminApiClient");
        }

        // Lists all release notes for all products
        public async Task<IActionResult> ListAllReleaseNotes()
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync("/ReleaseNotes/");

            if (!releaseNotesResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/ReleaseNotes/' failed");
            }

            var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
            var releaseNotes = JsonConvert.DeserializeObject<List<ReleaseNoteAdminApiModel>>(responseStream);

            var releaseNotesList = releaseNotes.Select(x => new ReleaseNoteAdminViewModel
            {
                Title = x.Title,
                BodyText = x.BodyText,
                Id = x.Id,
                ProductId = x.ProductId,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastUpdatedBy = x.LastUpdatedBy,
                LastUpdateDate = x.LastUpdateDate,
                IsDraft = x.IsDraft
            }).ToList();

            return View(releaseNotesList);
        }

        // Method for getting an release note object to delete
        public async Task<IActionResult> ViewReleaseNote(int Id)
        {
            var releaseNotesResult = await _releaseNotesClient.GetAsync($"/ReleaseNotes/{Id}");

            if (!releaseNotesResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/ReleaseNotes/' failed");
            }

            var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
            var releaseNote = JsonConvert.DeserializeObject<ReleaseNoteAdminViewModel>(responseStream);

            string[] PickedWorkItemId = new string[] { };

            if (releaseNote.PickedWorkItems != null)
            {
                PickedWorkItemId = releaseNote.PickedWorkItems.Split(' ');
                for (int i = 0; i < PickedWorkItemId.Length; i++)
                {
                    if (PickedWorkItemId[i].Length <= 1)
                    {
                        PickedWorkItemId = PickedWorkItemId.Take(PickedWorkItemId.Count() - 1).ToArray();
                    }
                }
            }

            List<WorkItemViewModel> workItemList = new List<WorkItemViewModel>();

            for (int i = 0; i < PickedWorkItemId.Length; i++)
            {
                var workItemResult = await _releaseNotesClient.GetAsync($"/WorkItem/{PickedWorkItemId[i]}");

                if (!workItemResult.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Get request to the URL 'API/WorkItem/' failed");
                }

                var responseStreamWorkItem = await workItemResult.Content.ReadAsStringAsync();
                var workItem = JsonConvert.DeserializeObject<WorkItemApiModel>(responseStreamWorkItem);

                var workItemViewModel = new WorkItemViewModel
                {
                    Id = workItem.Id,
                    Title = workItem.Title,
                    AssignedTo = workItem.AssignedTo,
                    State = workItem.State
                };

                workItemList.Add(workItemViewModel);
            }

            ViewBag.workItems = workItemList;

            var productsResult = await _releaseNotesClient.GetAsync($"/Product/{releaseNote.ProductId}");

            if (!productsResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/Product/' failed");
            }

            var responseStreamProduct = await productsResult.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ProductAdminViewModel>(responseStreamProduct);

            ViewBag.productName = product.ProductName;

            return View(releaseNote);
        }

        // Method for loading create-view
        public async Task<ActionResult> Create()
        {
            var productsResult = await _releaseNotesClient.GetAsync("/Product/");

            if (!productsResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/Product/' failed");
            }

            var responseStream = await productsResult.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductAdminApiModel>>(responseStream);

            var productsList = products.Select(x => new ProductAdminViewModel
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName
            }).ToList();

            ViewBag.products = productsList;

            //////

            var workItemResult = await _releaseNotesClient.GetAsync("/WorkItem/");

            if (!workItemResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/WorkItem/' failed");
            }

            var responseStreamWorkItem = await workItemResult.Content.ReadAsStringAsync();
            var workItems = JsonConvert.DeserializeObject<List<WorkItemApiModel>>(responseStreamWorkItem);

            var workItemList = workItems.Select(x => new WorkItemViewModel
            {
                Id = x.Id,
                Title = x.Title,
                AssignedTo = x.AssignedTo,
                State = x.State
            }).ToList();

            ViewBag.workitems = workItemList;

            return View();
        }

        // Method for creating release note
        public async Task<IActionResult> CreateReleaseNote(ReleaseNoteAdminApiModel releaseNote, string submitButton, string[] PickedWorkItems)
        {
            string PickedWorkItemsString = "";

            for (int i = 0; i < PickedWorkItems.Length; i++)
            {
                if (PickedWorkItems[i] != "false")
                {
                    PickedWorkItemsString += PickedWorkItems[i] + " ";
                }
            }

            if (PickedWorkItemsString == "")
            {
                PickedWorkItemsString = null;
                ModelState.AddModelError("PickedWorkItems", "You must select at least one related work item!");
            }

            string releaseNoteTitlePattern = @"^[a-zA-Z0-9, _ - ! ?. ""-]{3,100}$";
            var releaseNoteTitleMatch = Regex.Match(releaseNote.Title, releaseNoteTitlePattern, RegexOptions.IgnoreCase);
            if (!releaseNoteTitleMatch.Success)
            {
                ModelState.AddModelError("Title", "Title must be between three and one hundred characters!");
            }

            if (releaseNote.BodyText == null)
            {
                ModelState.AddModelError("BodyText", "Body text is required, and may not consist of zero characters!");
            }

            if (releaseNote.ProductId < 0)
            {
                ModelState.AddModelError("ProductId", "Product is required!");
            }

            string releaseNoteCreatedByPattern = @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$";
            var createdByMatch = Regex.Match(releaseNote.CreatedBy, releaseNoteCreatedByPattern, RegexOptions.IgnoreCase);
            if (!createdByMatch.Success)
            {
                ModelState.AddModelError("CreatedBy", "Author name may only consist of characters!");
            }

            if (!ModelState.IsValid)
            {
                TempData["CreateRN"] = "Failed";
                return View("Create");
            }

            //Encodes the bodytext so not raw html tags are inserted into the database
            //var EncodedBodyText = HttpUtility.HtmlEncode(releaseNote.BodyText);

            bool val = false;

            if (submitButton == "Save as draft")
            {
                val = true;
            }
            else if (submitButton == "Save and publish")
            {
                val = false;
            }

            var obj = new ReleaseNoteAdminApiModel
            {
                Title = releaseNote.Title,
                //Encodes the data from ckeditor
                BodyText = HttpUtility.HtmlEncode(releaseNote.BodyText),
                ProductId = releaseNote.ProductId,
                CreatedBy = releaseNote.CreatedBy,
                CreatedDate = DateTime.Now,
                IsDraft = val,
                PickedWorkItems = PickedWorkItemsString
            };

            var jsonString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await _releaseNotesClient.PostAsync("/ReleaseNotes/", content);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed creating Release Note with title; {obj.Title}");
            }

            TempData["CreateRN"] = "Success";
            return RedirectToAction("ListAllReleaseNotes");
        }

        // Method for getting release note object to edit
        public async Task<IActionResult> EditReleaseNote(int Id)
        {
            // Getting data for Release notes
            var releaseNotesResult = await _releaseNotesClient.GetAsync($"/ReleaseNotes/{Id}");

            if (!releaseNotesResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/ReleaseNotes/' failed");
            }

            var responseStreamReleaseNote = await releaseNotesResult.Content.ReadAsStringAsync();
            var releaseNote = JsonConvert.DeserializeObject<ReleaseNoteAdminApiModel>(responseStreamReleaseNote);

            var releaseNoteViewModel = new ReleaseNoteAdminViewModel
            {
                Title = releaseNote.Title,
                BodyText = HttpUtility.HtmlDecode(releaseNote.BodyText),
                ProductId = releaseNote.ProductId,
                CreatedBy = releaseNote.CreatedBy,
                CreatedDate = releaseNote.CreatedDate,
                LastUpdatedBy = releaseNote.LastUpdatedBy,
                LastUpdateDate = DateTime.Now,
                IsDraft = releaseNote.IsDraft
            };

            ViewBag.selectedWorkItems = releaseNote.PickedWorkItems;

            // Getting data for Product
            var productsResult = await _releaseNotesClient.GetAsync("/Product/");

            if (!productsResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/Product/' failed");
            }

            var responseStreamProduct = await productsResult.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductAdminApiModel>>(responseStreamProduct);

            //Lists all the products the admin can choose
            var productsList = products.Select(x => new ProductAdminViewModel
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
            }).ToList();

            ViewBag.products = productsList;

            var workItemResult = await _releaseNotesClient.GetAsync("/WorkItem/");
            var responseStreamWorkItem = await workItemResult.Content.ReadAsStringAsync();
            var workItems = JsonConvert.DeserializeObject<List<WorkItemApiModel>>(responseStreamWorkItem);

            var workItemList = workItems.Select(x => new WorkItemViewModel
            {
                Id = x.Id,
                Title = x.Title,
                AssignedTo = x.AssignedTo,
                State = x.State
            }).ToList();

            ViewBag.workitems = workItemList;

            return View(releaseNoteViewModel);
        }

        // Method for posting edit on a release note object
        [HttpPost]
        public async Task<IActionResult> EditReleaseNote(int Id, ReleaseNoteAdminViewModel releaseNote, string submitButton, string[] PickedWorkItems)
        {
            string releaseNoteTitlePattern = @"^[a-zA-Z0-9, _ - ! ?. ""-]{3,100}$";
            var releaseNoteTitleMatch = Regex.Match(releaseNote.Title, releaseNoteTitlePattern, RegexOptions.IgnoreCase);
            if (!releaseNoteTitleMatch.Success)
            {
                ModelState.AddModelError("Title", "Title must be between six and one hundred characters!");
            }

            if (releaseNote.BodyText == null)
            {
                ModelState.AddModelError("BodyText", "Body text is required, and may not consist of zero characters!");
            }

            if (releaseNote.ProductId < 0)
            {
                ModelState.AddModelError("ProductId", "Product is required!");
            }

            string releaseNoteCreatedByPattern = @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$";
            var createdByMatch = Regex.Match(releaseNote.CreatedBy, releaseNoteCreatedByPattern, RegexOptions.IgnoreCase);
            if (!createdByMatch.Success)
            {
                ModelState.AddModelError("CreatedBy", "Author name may only consist of characters!");
            }

            string releaseNoteLastUpdatedByPattern = @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$";
            var lastUpdatedByMatch = Regex.Match(releaseNote.LastUpdatedBy, releaseNoteLastUpdatedByPattern, RegexOptions.IgnoreCase);
            if (!lastUpdatedByMatch.Success)
            {
                ModelState.AddModelError("LastUpdatedBy", "Last updated by may only consist of characters!");
            }

            string PickedWorkItemsString = "";

            for (int i = 0; i < PickedWorkItems.Length; i++)
            {
                if (PickedWorkItems[i] != "false")
                {
                    PickedWorkItemsString += PickedWorkItems[i] + " ";
                }
            }

            if (PickedWorkItemsString == "")
            {
                PickedWorkItemsString = null;
                ModelState.AddModelError("PickedWorkItems", "You must select at least one related work item!");
            }

            bool val = false;

            if (submitButton == "Save as draft")
            {
                val = true;
            }
            else if (submitButton == "Save and publish")
            {
                val = false;
            }

            releaseNote.IsDraft = val;
            releaseNote.PickedWorkItems = PickedWorkItemsString;

            var jsonString = JsonConvert.SerializeObject(releaseNote);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await _releaseNotesClient.PutAsync($"/ReleaseNotes/{Id}", content);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Couldn't edit release note with id = {Id}");
            }

            if (!ModelState.IsValid)
            {
                TempData["EditRN"] = "Failed";
                return View("EditReleaseNote");
            }

            TempData["EditRN"] = "Success";
            return RedirectToAction("ViewReleaseNote", new { id = Id });
        }

        // Method for deleting object
        [HttpPost]
        public async Task<IActionResult> DeleteReleaseNote(int Id)
        {
            var transportData = await _releaseNotesClient.DeleteAsync($"/ReleaseNotes/{Id}");

            if (!transportData.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Couldnt delete release note with id = {Id}");
            }

            TempData["DeleteRN"] = "Success";
            return RedirectToAction("ListAllReleaseNotes");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
