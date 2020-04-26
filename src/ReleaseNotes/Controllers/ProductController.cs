using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotes.Models;
using ReleaseNotes.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ReleaseNotes.Controllers
{
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _productsClient;

        public ProductController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _productsClient = _httpClientFactory.CreateClient("ReleaseNotesApiClient");
        }

        // Loading all products for Talentech on the front-page
        public async Task<IActionResult> ListAllProducts()
        {
            var productResult = await _productsClient.GetAsync("/Product/");

            if (!productResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/Product/' failed");
            }

            var responseStream = await productResult.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ProductApiModel>>(responseStream);

            var productsList = products.Select(x => new ProductViewModel
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage
            }).ToList();

            return View(productsList);
        }
    }
}