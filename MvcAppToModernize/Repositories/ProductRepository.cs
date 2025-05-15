using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Models;

namespace Repositories
{
    /// <summary>
    /// Repository class that handles persistence of Product data using JSON file storage
    /// Implements IProductRepository interface for data access operations
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        // File path where products JSON data will be stored
        private readonly string _jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "products.json");

        /// <summary>
        /// Constructor that ensures the data directory and file exist
        /// Creates them if they don't exist
        /// </summary>
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

        /// <summary>
        /// Retrieves all products from the JSON file
        /// Returns empty list if no products exist
        /// </summary>
        /// <returns>List of all Product objects</returns>
        public List<Product> GetAllProducts()
        {
            string jsonContent = File.ReadAllText(_jsonFilePath);
            return JsonConvert.DeserializeObject<List<Product>>(jsonContent) ?? new List<Product>();
        }

        /// <summary>
        /// Finds and returns a specific product by its ID
        /// </summary>
        /// <param name="productId">The ID of the product to find</param>
        /// <returns>Product object if found, null if not found</returns>
        public Product GetProductById(int productId)
        {
            var products = GetAllProducts();
            return products.Find(p => p.ProductId == productId);
        }

        /// <summary>
        /// Saves the provided list of products to the JSON file
        /// Overwrites any existing data
        /// </summary>
        /// <param name="products">List of products to save</param>
        public void SaveProducts(List<Product> products)
        {
            string jsonContent = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(_jsonFilePath, jsonContent);
        }
    }
}
