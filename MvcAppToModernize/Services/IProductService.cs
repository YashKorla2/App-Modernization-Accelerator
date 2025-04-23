using System.Collections.Generic;
using Models;

namespace Services
{
    /// <summary>
    /// Interface defining the contract for product management operations
    /// Provides methods for basic CRUD operations and product search functionality
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all products from the data store
        /// </summary>
        /// <returns>List of all available products</returns>
        List<Product> GetAllProducts();

        /// <summary>
        /// Retrieves a specific product by its ID
        /// </summary>
        /// <param name="productId">The unique identifier of the product</param>
        /// <returns>The product if found, null otherwise</returns>
        Product GetProductById(int productId);

        /// <summary>
        /// Adds a new product to the data store
        /// </summary>
        /// <param name="product">The product entity to be added</param>
        void AddProduct(Product product);

        /// <summary>
        /// Updates an existing product in the data store
        /// </summary>
        /// <param name="updatedProduct">The product entity with updated values</param>
        void UpdateProduct(Product updatedProduct);

        /// <summary>
        /// Removes a product from the data store
        /// </summary>
        /// <param name="productId">The unique identifier of the product to delete</param>
        void DeleteProduct(int productId);

        /// <summary>
        /// Searches for products based on a search term
        /// </summary>
        /// <param name="searchTerm">The search criteria</param>
        /// <returns>List of products matching the search criteria</returns>
        List<Product> SearchProducts(string searchTerm);
    }
}
