using System;
using System.Collections.Generic;
using Models;

namespace Repositories
{
    public interface ICartRepository
    {
        List<Cart> GetCart();
        void SaveCart(List<Cart> cart);
        void AddToCart(Cart item);
        List<List<Cart>> GetOrders();
        void SaveOrders(List<List<Cart>> orders);
    }

}