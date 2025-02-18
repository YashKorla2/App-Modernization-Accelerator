using System;
using System.Collections.Generic;
using Models;

namespace Services
{
    public interface ICartService
    {
        void AddProductToCart(Product product, int quantity);
        List<Cart> GetCarts();
        void DeleteCartItem(int itemId);
        List<List<Cart>> GetOrders();
        void Checkout(int[] selectedProductIds);
        List<Cart> SearchCart(string searchTerm);
        List<List<Cart>> SearchOrders(string searchTerm);
    }
}