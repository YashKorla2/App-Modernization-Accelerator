using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Models;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "products.json");

        public ProductRepository()
        {
            string directory = Path.GetDirectoryName(_jsonFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (!File.Exists(_jsonFilePath))
            {
                File.WriteAllText(_jsonFilePath, "[]");
            }
        }

        public List<Product> GetAllProducts()
        {
            string jsonContent = File.ReadAllText(_jsonFilePath);
            return JsonConvert.DeserializeObject<List<Product>>(jsonContent) ?? new List<Product>();
        }

        public Product GetProductById(int productId)
        {
            var products = GetAllProducts();
            return products.Find(p => p.ProductId == productId);
        }

        public void SaveProducts(List<Product> products)
        {
            string jsonContent = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(_jsonFilePath, jsonContent);
        }
    }
}
