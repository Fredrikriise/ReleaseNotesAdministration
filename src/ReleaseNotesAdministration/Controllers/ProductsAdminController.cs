using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReleaseNotesAdministration.Models;
using ReleaseNotesAdministration.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<IActionResult> ListAllProducts()
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
            if (product.ProductName == null)
            {
                ModelState.AddModelError("ProductName", "Product name is required!");
            }
            //Here we should add regex to check if the format of ProductImage is correct, currently anything is accepted with this statement
            if (product.ProductImage == null)
            {
                ModelState.AddModelError("ProductImage", "Product image is required!");
            } 
            //Here we should also add regex, or just manually assign the variable to be product name without white spaces
            //Or, if we make a method to get the product by id, then we dont even need product description, as product description is only used for the views 
            if (product.ProductDescription == null)
            {
                ModelState.AddModelError("ProductDescription", "Product description is required!");
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
                ProductDescription = product.ProductDescription
            };

            var jsonString = JsonConvert.SerializeObject(obj);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            await _releaseNotesClient.PostAsync("/Product/", content);

            TempData["CreateProduct"] = "Success";
            return RedirectToAction("ListAllProducts");
        }

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

                if (product.ProductName == null)
                {
                    ModelState.AddModelError("ProductName", "Product name is required!");
                }
                //Here we should add regex to check if the format of ProductImage is correct, currently anything is accepted with this statement
                if (product.ProductImage == null)
                {
                    ModelState.AddModelError("ProductImage", "Product image is required!");
                }
                //Here we should also add regex, or just manually assign the variable to be product name without white spaces
                //Or, if we make a method to get the product by id, then we dont even need product description, as product description is only used for the views 
                if (product.ProductDescription == null)
                {
                    ModelState.AddModelError("ProductDescription", "Product description is required!");
                }

                if(!ModelState.IsValid) {
                    TempData["EditProduct"] = "Failed";
                    return View("EditProduct");
                }

                
                TempData["EditProduct"] = "Success";
                return RedirectToAction("ViewProduct", new { id = Id });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Method for getting an product object to delete
        public async Task<IActionResult> ViewProduct(int Id)
        {
            var productsResult = await _releaseNotesClient.GetAsync($"/Product/{Id}");

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
        public async Task<IActionResult> DeleteProduct(int? Id)
        {
            try
            {
                var transportData = await _releaseNotesClient.DeleteAsync($"/Product/{Id}");

                TempData["DeleteProduct"] = "Success";
                return RedirectToAction("ListAllProducts");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}