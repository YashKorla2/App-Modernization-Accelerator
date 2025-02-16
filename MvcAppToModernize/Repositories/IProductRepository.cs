using System.Collections.Generic;
using Models;

namespace Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAllProducts();
        Product GetProductById(int productId);
        void SaveProducts(List<Product> products);
    }
}
