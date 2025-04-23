// Interface defining the contract for product data access operations
using System.Collections.Generic;
using Models;

namespace Repositories
{
    public interface IProductRepository
    {
        // Retrieves all products from the data store
        List<Product> GetAllProducts();

        // Gets a specific product by its unique identifier
        Product GetProductById(int productId);

        // Persists a list of products to the data store
        void SaveProducts(List<Product> products);
    }
}
