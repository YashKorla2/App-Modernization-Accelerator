using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using ExternalAPI;
using Newtonsoft.Json;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly ExternalApi _externalApi;
        private const string FileName = "products.json";

        public ProductService(ExternalApi externalApi)
        {
            _externalApi = externalApi;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            string jsonData = await _externalApi.ReadFileAsync(FileName);
            return string.IsNullOrEmpty(jsonData) ? new List<Product>() : JsonConvert.DeserializeObject<List<Product>>(jsonData);
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var products = await GetAllProductsAsync();
            return products.FirstOrDefault(p => p.ProductId == productId);
        }

        public async Task AddProductAsync(Product product)
        {
            var products = await GetAllProductsAsync();
            int maxId = products.Any() ? products.Max(p => p.ProductId) : 0;
            product.ProductId = maxId + 1;
            products.Add(product);
            await _externalApi.UploadFileAsync(FileName, products);
        }

        public async Task UpdateProductAsync(Product updatedProduct)
        {
            var products = await GetAllProductsAsync();
            int index = products.FindIndex(p => p.ProductId == updatedProduct.ProductId);
            if (index != -1)
            {
                products[index] = updatedProduct;
                await _externalApi.UploadFileAsync(FileName, products);
            }
        }

        public async Task DeleteProductAsync(int productId)
        {
            var products = await GetAllProductsAsync();
            products.RemoveAll(p => p.ProductId == productId);
            await _externalApi.UploadFileAsync(FileName, products);
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            var products = await GetAllProductsAsync();
            return products.Where(
                p => p.ProductName.ToLower().Contains(searchTerm.ToLower()) ||
                     p.ProductDescription.ToLower().Contains(searchTerm.ToLower()) ||
                     p.ProductCategory.ToLower().Contains(searchTerm.ToLower())
            ).ToList();
        }
    }
}
