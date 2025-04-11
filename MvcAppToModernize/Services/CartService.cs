using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using ExternalAPI;
using Newtonsoft.Json;

namespace Services
{
    public class CartService : ICartService
    {
        private readonly ExternalApi _externalApi;
        private const string CartFileName = "cart.json";
        private const string OrdersFileName = "orders.json";

        public CartService(ExternalApi externalApi)
        {
            _externalApi = externalApi;
        }

        public async Task<List<Cart>> GetCartsAsync()
        {
            string jsonData = await _externalApi.ReadFileAsync(CartFileName);
            return string.IsNullOrEmpty(jsonData) ? new List<Cart>() : JsonConvert.DeserializeObject<List<Cart>>(jsonData);
        }

        public async Task AddProductToCartAsync(Product product, int quantity)
        {
            var cartItems = await GetCartsAsync();

            var existingItem = cartItems.FirstOrDefault(c => c.ProductId == product.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newCartItem = new Cart
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice,
                    Quantity = quantity
                };
                cartItems.Add(newCartItem);
            }

            await _externalApi.UploadFileAsync(CartFileName, cartItems);
        }

        public async Task DeleteCartItemAsync(int itemId)
        {
            var cartItems = await GetCartsAsync();
            cartItems.RemoveAll(item => item.ProductId == itemId);
            await _externalApi.UploadFileAsync(CartFileName, cartItems);
        }

        public async Task<List<List<Cart>>> GetOrdersAsync()
        {
            string jsonData = await _externalApi.ReadFileAsync(OrdersFileName);
            return string.IsNullOrEmpty(jsonData) ? new List<List<Cart>>() : JsonConvert.DeserializeObject<List<List<Cart>>>(jsonData);
        }

        public async Task CheckoutAsync(int[] selectedProductIds)
        {
            var cartItems = await GetCartsAsync();
            var selectedItems = cartItems.Where(c => selectedProductIds.Contains(c.ProductId)).ToList();

            if (selectedItems.Count == 0) return;

            var orders = await GetOrdersAsync();
            orders.Add(selectedItems);

            await _externalApi.UploadFileAsync(OrdersFileName, orders);

            cartItems.RemoveAll(c => selectedProductIds.Contains(c.ProductId));
            await _externalApi.UploadFileAsync(CartFileName, cartItems);
        }

        public async Task<List<Cart>> SearchCartAsync(string searchTerm)
        {
            var carts = await GetCartsAsync();
            return carts.Where(cart => cart.ProductName.ToLower().Contains(searchTerm.ToLower())).ToList();
        }

        public async Task<List<List<Cart>>> SearchOrdersAsync(string searchTerm)
        {
            var orders = await GetOrdersAsync();
            return orders
                .Select(orderList => orderList
                    .Where(cart => cart.ProductName.ToLower().Contains(searchTerm.ToLower()))
                    .ToList()
                )
                .Where(filteredList => filteredList.Count > 0)
                .ToList();
        }
    }
}
