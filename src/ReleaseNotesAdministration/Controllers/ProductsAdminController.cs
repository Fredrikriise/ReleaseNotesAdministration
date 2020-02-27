using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        // Method for listing all products
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

        // Method for loading create-view
        public ActionResult Create()
        {
            return View();
        }

        // Method for creating product
        public async Task<IActionResult> CreateProduct(ProductAdminApiModel product)
        {
            var obj = new ProductAdminApiModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductImage = product.ProductImage,
                ProductDescription = product.ProductDescription
            };

            var jsonString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            await _releaseNotesClient.PostAsync("/Product/", content);

            return RedirectToAction("ListProducts");
        }


        // --------------------------------------------------------------------------------------


        // Method for getting product object to edit
        public async Task<IActionResult> EditProduct(int Id)
        {
            var productsResult = await _releaseNotesClient.GetAsync($"/Product/{Id}");

            if (!productsResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/Product/' failed");
            }

            var responseStream = await productsResult.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ProductAdminApiModel>(responseStream);

            var productViewModel = new ProductAdminViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductImage = product.ProductImage,
                ProductDescription = product.ProductDescription
            };

            return View(productViewModel);
        }

        // Method for posting edit on a product object
        [HttpPost]
        public async Task<IActionResult> EditProduct(int? Id, ProductAdminViewModel product)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(product);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var transportData = await _releaseNotesClient.PutAsync($"/Product/{Id}", content);
                return RedirectToAction("ListProducts");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }





    }
}