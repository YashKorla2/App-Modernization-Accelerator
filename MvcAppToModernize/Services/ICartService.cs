using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface ICartService
    {
        Task<List<Cart>> GetCartsAsync();
        Task AddProductToCartAsync(Product product, int quantity);
        Task DeleteCartItemAsync(int itemId);
        Task<List<List<Cart>>> GetOrdersAsync();
        Task CheckoutAsync(int[] selectedProductIds);
        Task<List<Cart>> SearchCartAsync(string searchTerm);
        Task<List<List<Cart>>> SearchOrdersAsync(string searchTerm);
    }
}
