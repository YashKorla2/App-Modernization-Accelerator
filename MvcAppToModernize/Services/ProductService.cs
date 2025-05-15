using System.Collections.Generic;
using System.Linq;
using Models;
using Repositories;

namespace Services
{
    /// <summary>
    /// Service class that handles business logic for Product operations
    /// Implements IProductService interface and uses IProductRepository for data access
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Constructor that injects IProductRepository dependency
        /// </summary>
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Retrieves all products from the repository
        /// </summary>
        /// <returns>List of all products</returns>
        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        /// <summary>
        /// Retrieves a specific product by its ID
        /// </summary>
        /// <param name="productId">ID of the product to retrieve</param>
        /// <returns>Product matching the ID</returns>
        public Product GetProductById(int productId)
        {
            return _productRepository.GetProductById(productId);
        }

        /// <summary>
        /// Adds a new product to the repository
        /// Automatically generates a new product ID by finding the maximum existing ID and adding 1
        /// </summary>
        /// <param name="product">Product to be added</param>
        public void AddProduct(Product product)
        {
            var products = _productRepository.GetAllProducts();
            int maxId = products.Any() ? products.Max(p => p.ProductId) : 0;
            product.ProductId = maxId + 1;
            products.Add(product);
            _productRepository.SaveProducts(products);
        }

        /// <summary>
        /// Updates an existing product in the repository
        /// Finds the product by ID and replaces it with the updated version
        /// </summary>
        /// <param name="updatedProduct">Product with updated information</param>
        public void UpdateProduct(Product updatedProduct)
        {
            var products = _productRepository.GetAllProducts();
            int index = products.FindIndex(p => p.ProductId == updatedProduct.ProductId);
            if (index != -1)
            {
                products[index] = updatedProduct;
                _productRepository.SaveProducts(products);
            }
        }

        /// <summary>
        /// Deletes a product from the repository by its ID
        /// </summary>
        /// <param name="productId">ID of the product to delete</param>
        public void DeleteProduct(int productId)
        {
            var products = _productRepository.GetAllProducts();
            products.RemoveAll(p => p.ProductId == productId);
            _productRepository.SaveProducts(products);
        }

        /// <summary>
        /// Searches for products based on a search term
        /// Searches across product name, description, and category (case-insensitive)
        /// </summary>
        /// <param name="searchTerm">Term to search for</param>
        /// <returns>List of products matching the search term</returns>
        public List<Product> SearchProducts(string searchTerm)
        {
            var products = _productRepository.GetAllProducts();
            return products.Where(
                p => p.ProductName.ToLower().Contains(searchTerm.ToLower()) || 
                p.ProductDescription.ToLower().Contains(searchTerm.ToLower()) ||
                p.ProductCategory.ToLower().Contains(searchTerm.ToLower())
            ).ToList();
        }
    }
}
