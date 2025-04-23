using System;
using System.Collections.Generic;
using Models;

namespace Repositories
{
    /// <summary>
    /// Interface defining repository operations for managing shopping cart and orders
    /// </summary>
    public interface ICartRepository
    {
        /// <summary>
        /// Retrieves the current shopping cart
        /// </summary>
        /// <returns>List of cart items</returns>
        List<Cart> GetCart();

        /// <summary>
        /// Saves the current state of the shopping cart
        /// </summary>
        /// <param name="cart">List of cart items to save</param>
        void SaveCart(List<Cart> cart);

        /// <summary>
        /// Adds a new item to the shopping cart
        /// </summary>
        /// <param name="item">Cart item to add</param>
        void AddToCart(Cart item);

        /// <summary>
        /// Retrieves the order history
        /// </summary>
        /// <returns>List of orders, where each order is a list of cart items</returns>
        List<List<Cart>> GetOrders();

        /// <summary>
        /// Saves the order history
        /// </summary>
        /// <param name="orders">List of orders to save</param>
        void SaveOrders(List<List<Cart>> orders);
    }

}
