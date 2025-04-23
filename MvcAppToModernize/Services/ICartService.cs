using System;
using System.Collections.Generic;
using Models;

namespace Services
{
    /// <summary>
    /// Interface defining the contract for cart management operations
    /// Handles shopping cart functionality including adding/removing items and checkout
    /// </summary>
    public interface ICartService
    {
        /// <summary>
        /// Adds a product to the shopping cart with specified quantity
        /// </summary>
        /// <param name="product">The product to add</param>
        /// <param name="quantity">Number of items to add</param>
        void AddProductToCart(Product product, int quantity);

        /// <summary>
        /// Retrieves all active shopping carts in the system
        /// </summary>
        /// <returns>List of shopping cart items</returns>
        List<Cart> GetCarts();

        /// <summary>
        /// Removes a specific item from the shopping cart
        /// </summary>
        /// <param name="itemId">ID of the cart item to remove</param>
        void DeleteCartItem(int itemId);

        /// <summary>
        /// Retrieves order history showing completed purchases
        /// </summary>
        /// <returns>List of completed orders, where each order contains cart items</returns>
        List<List<Cart>> GetOrders();

        /// <summary>
        /// Processes checkout for selected products in the cart
        /// </summary>
        /// <param name="selectedProductIds">Array of product IDs to checkout</param>
        void Checkout(int[] selectedProductIds);

        /// <summary>
        /// Searches active cart items based on search criteria
        /// </summary>
        /// <param name="searchTerm">Term to search for in cart items</param>
        /// <returns>List of matching cart items</returns>
        List<Cart> SearchCart(string searchTerm);

        /// <summary>
        /// Searches completed orders based on search criteria
        /// </summary>
        /// <param name="searchTerm">Term to search for in orders</param>
        /// <returns>List of orders containing matching items</returns>
        List<List<Cart>> SearchOrders(string searchTerm);
    }
}
