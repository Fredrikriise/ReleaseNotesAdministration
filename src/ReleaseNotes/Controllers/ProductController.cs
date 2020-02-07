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

            _productsClient = _httpClientFactory.CreateClient("ReleaseNotesApiClient");
        }

        public async Task<IActionResult> Index()
        {
            //Skal vi bare hardkode produktid'en i "url"'en?
            var productResult = await _productsClient.GetAsync("/Product/Index");
            var productList = new List<ProductViewModel>();

            if (productResult.IsSuccessStatusCode)
            {
                var responseStream = await productResult.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<ProductList>(responseStream);

                foreach (var product in products.Products)
                {
                    var productVm = new ProductViewModel()
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductImage = product.ProductImage,
                        ProductDescription = product.ProductDescription
                    };
                    productList.Add(productVm);

                    // For debug
                    Console.WriteLine(product);
                }
            }
            else
            {

            }

            return View(productList);
        }
    }
}