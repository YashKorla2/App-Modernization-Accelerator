using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Models;

namespace Services
{
    public class HomeService : IHomeService
    {
        private readonly string _jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "products.json");

        public HomeService()
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

        public void AddProduct(Product product)
        {
            var products = GetAllProducts();
            products.Add(product);
            SaveProducts(products);
        }

        public void UpdateProduct(Product updatedProduct)
        {
            var products = GetAllProducts();
            int index = products.FindIndex(p => p.ProductId == updatedProduct.ProductId);
            if (index != -1)
            {
                products[index] = updatedProduct;
                SaveProducts(products);
            }
        }

        public void DeleteProduct(int productId)
        {
            var products = GetAllProducts();
            products.RemoveAll(p => p.ProductId == productId);
            SaveProducts(products);
        }

        private void SaveProducts(List<Product> products)
        {
            string jsonContent = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(_jsonFilePath, jsonContent);
        }
        // Add your service methods here
    }
}