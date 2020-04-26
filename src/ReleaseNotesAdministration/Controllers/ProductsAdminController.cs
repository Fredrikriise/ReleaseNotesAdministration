using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotesAdministration.Models;
using ReleaseNotesAdministration.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReleaseNotesAdministration.Controllers
{
    public class ProductsAdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _productsClient;

        public ProductsAdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _productsClient = _httpClientFactory.CreateClient("ReleaseNotesAdminApiClient");
        }

        // Method for listing all products
        public async Task<IActionResult> ListAllProducts()
        {
            var productsResult = await _productsClient.GetAsync("/Product/");

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
            string productNamePattern = @"^[A-Za-z0-9\s\-_,\.;:!()+']{3,99}$";
            var productNameMatch = Regex.Match(product.ProductName, productNamePattern, RegexOptions.IgnoreCase);
            if (!productNameMatch.Success)
            {
                ModelState.AddModelError("ProductName", "Product may only contain numbers and characters!");
            }

            string productImagePattern = @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$";
            var productImageMatch = Regex.Match(product.ProductImage, productImagePattern, RegexOptions.IgnoreCase);
            if (!productImageMatch.Success)
            {
                ModelState.AddModelError("ProductImage", "Product image must be either .jpg, .jpeg or .png file!");
            }

            if (!ModelState.IsValid)
            {
                TempData["CreateProduct"] = "Failed";
                return View("Create");
            }

            var obj = new ProductAdminApiModel
            {
                ProductName = product.ProductName,
                ProductImage = product.ProductImage,
            };

            var jsonString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var result = await _productsClient.PostAsync("/Product/", content);

            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Failed creating product");
            }

            TempData["CreateProduct"] = "Success";
            return RedirectToAction("ListAllProducts");
        }

        // Method for getting product object to edit
        public async Task<IActionResult> EditProduct(int Id)
        {
            var productResult = await _productsClient.GetAsync($"/Product/{Id}");

            if (!productResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/Product/' failed");
            }

            var responseStream = await productResult.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ProductAdminApiModel>(responseStream);

            var productViewModel = new ProductAdminViewModel
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductImage = product.ProductImage
            };

            return View(productViewModel);
        }

        // Method for posting edit on a product object
        [HttpPost]
        public async Task<IActionResult> EditProduct(int Id, ProductAdminViewModel product)
        {
            var jsonString = JsonConvert.SerializeObject(product);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var transportData = await _productsClient.PutAsync($"/Product/{Id}", content);

            if (!transportData.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Editing product with id = {Id} failed.");
            }

            string productNamePattern = @"^[a-zA-Z0-9, _ - ! ?. ""]*$";
            var productNameMatch = Regex.Match(product.ProductName, productNamePattern, RegexOptions.IgnoreCase);
            if (!productNameMatch.Success)
            {
                ModelState.AddModelError("ProductName", "Product name is required, and may only contain numbers and characters!");
            }

            string productImagePattern = @"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$";
            var productImageMatch = Regex.Match(product.ProductImage, productImagePattern, RegexOptions.IgnoreCase);
            if (!productImageMatch.Success)
            {
                ModelState.AddModelError("ProductImage", "Product image is required, and must be either .jpg, .jpeg or .png file!");
            }

            if (!ModelState.IsValid)
            {
                TempData["EditProduct"] = "Failed";
                return View("EditProduct");
            }

            TempData["EditProduct"] = "Success";
            return RedirectToAction("ViewProduct", new { id = Id });
        }

        // Method for getting an product object to view
        public async Task<IActionResult> ViewProduct(int Id)
        {
            var productsResult = await _productsClient.GetAsync($"/Product/{Id}");

            if (!productsResult.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Get request to the URL 'API/Product/' failed");
            }

            var responseStream = await productsResult.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ProductAdminViewModel>(responseStream);
            return View(product);
        }

        // Method for deleting object
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            var transportData = await _productsClient.DeleteAsync($"/Product/{Id}");

            if (!transportData.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Couldn't delete product with id = {Id}");
            }

            TempData["DeleteProduct"] = "Success";
            return RedirectToAction("ListAllProducts");
        }
    }
}