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
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _productsClient;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _productsClient = _httpClientFactory.CreateClient("ProductApiClient");
        }

        public async Task<IActionResult> TalentRecruiter()
        {
            //Skal vi bare hardkode produktid'en i "url"'en?
            var releaseNotesResult = await _productsClient.GetAsync("/Product/1");
            var list = new List<ProductViewModel>();

            if (releaseNotesResult.IsSuccessStatusCode)
            {
                var responseStream = await releaseNotesResult.Content.ReadAsStringAsync();
                var releaseNotes = JsonConvert.DeserializeObject<ProductList>(responseStream);

                foreach (var releaseNote in releaseNotes.Products)
                {
                    var releaseNoteVm = new ProductViewModel()
                    {
                        ProductId = releaseNote.ProductId,
                        ProductName = releaseNote.ProductName,
                        ProductImage = releaseNote.ProductImage,
                        ProductDescription = releaseNote.ProductDescription
                    };

                    list.Add(releaseNoteVm);

                    // For debug
                    Console.WriteLine(releaseNote);
                }
            }
            else
            {

            }

            return View(list);
        }
    }
}