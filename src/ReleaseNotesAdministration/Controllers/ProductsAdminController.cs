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
    public class ProductsAdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _releaseNotesClient;

        public ProductsAdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _releaseNotesClient = _httpClientFactory.CreateClient("ReleaseNotesAdminApiClient");
        }

        // Lists all release notes for all products
        public async Task<IActionResult> ListProducts()
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
                ProductImage = x.ProductImage,
                ProductDescription = x.ProductDescription
            }).ToList();

            return View(productsList);
        }
    }
}