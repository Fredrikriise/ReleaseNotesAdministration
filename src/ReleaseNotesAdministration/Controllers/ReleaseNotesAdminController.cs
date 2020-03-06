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
using System.Threading.Tasks;

namespace ReleaseNotesAdministration.Controllers
{
    public class ReleaseNotesAdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _releaseNotesClient;

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
                LastUpdateDate = x.LastUpdateDate
            }).ToList();

            return View(releaseNotesList);
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
                ProductName = x.ProductName,
            }).ToList();

            ViewBag.products = productsList;
            return View();
        }

        // Method for creating release note
        public async Task<IActionResult> CreateReleaseNote(ReleaseNoteAdminApiModel releaseNote)
        {
            if (releaseNote.Title == null)
            {
                ModelState.AddModelError("Title", "Title is required!");
            } 
            
            if (releaseNote.BodyText == null)
            {
                ModelState.AddModelError("BodyText", "Body text is required!");
            }
            
            if (releaseNote.ProductId < 0)
            {
                ModelState.AddModelError("ProductId", "Product is required!");
            } 
            
            if (releaseNote.CreatedBy == null)
            {
                ModelState.AddModelError("CreatedBy", "Author name is required!");
            }

            if (!ModelState.IsValid)
            {
                TempData["CreateRN"] = "Failed"; 
                return View("Create");
            }

            var obj = new ReleaseNoteAdminApiModel
            {
                Title = releaseNote.Title,
                BodyText = releaseNote.BodyText,
                ProductId = releaseNote.ProductId,
                CreatedBy = releaseNote.CreatedBy,
                CreatedDate = DateTime.Now
            };

            var jsonString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            await _releaseNotesClient.PostAsync("/ReleaseNotes/", content);

            TempData["CreateRN"] = "Added";
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
                BodyText = releaseNote.BodyText,
                ProductId = releaseNote.ProductId,
                CreatedBy = releaseNote.CreatedBy,
                CreatedDate = releaseNote.CreatedDate,
                LastUpdatedBy = releaseNote.LastUpdatedBy,
                LastUpdateDate = DateTime.Now
            };

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

            return View(releaseNoteViewModel);
        }

        // Method for posting edit on a release note object
        [HttpPost]
        public async Task<IActionResult> EditReleaseNote(int? Id, ReleaseNoteAdminViewModel releaseNote)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(releaseNote);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var transportData = await _releaseNotesClient.PutAsync($"/ReleaseNotes/{Id}", content);

                
                if(releaseNote.Title.Length == 0)
                {
                    ModelState.AddModelError("Title", "Title is required!");
                } 
                if (releaseNote.BodyText == null)
                {
                    ModelState.AddModelError("BodyText", "Body text is required!");
                } 
                if (releaseNote.ProductId < 0)
                {
                    ModelState.AddModelError("ProductId", "Product is required!");
                }
                if (releaseNote.CreatedBy == null)
                {
                    ModelState.AddModelError("CreatedBy", "Author is required!");
                } 
                if (releaseNote.LastUpdatedBy == null)
                {
                    ModelState.AddModelError("LastUpdatedBy", "Last updated by is required!");
                }

                if(!ModelState.IsValid)
                {
                    TempData["EditRN"] = "Failed";
                    return View("EditReleaseNote");
                }

                TempData["EditRN"] = "Success";
                return RedirectToAction("ViewReleaseNote", new { id = Id}); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
            return View(releaseNote);
        }

        // Method for deleting object
        [HttpPost]
        public async Task<IActionResult> DeleteReleaseNote(int? Id)
        {
            try
            {
                var transportData = await _releaseNotesClient.DeleteAsync($"/ReleaseNotes/{Id}");

                TempData["DeleteRN"] = "Success";
                return RedirectToAction("ListAllReleaseNotes"); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
