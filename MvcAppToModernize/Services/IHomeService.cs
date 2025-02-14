using System.Collections.Generic;
using Models;

namespace Services
{
    public interface IHomeService
    {
        List<Product> GetAllProducts();
        Product GetProductById(int productId);
        void AddProduct(Product product);
        void UpdateProduct(Product updatedProduct);
        void DeleteProduct(int productId);
    }
}
