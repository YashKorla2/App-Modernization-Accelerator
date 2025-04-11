using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product updatedProduct);
        Task DeleteProductAsync(int productId);
        Task<List<Product>> SearchProductsAsync(string searchTerm);
    }
}
