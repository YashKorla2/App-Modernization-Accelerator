using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Repositories;

namespace Services
{
    /// <summary>
    /// Service class that handles shopping cart operations including managing cart items and orders
    /// Provides functionality for adding/removing items, checkout, and searching
    /// </summary>
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        /// <summary>
        /// Constructor that injects cart repository dependency
        /// </summary>
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        /// <summary>
        /// Retrieves all items currently in the shopping cart
        /// </summary>
        public List<Cart> GetCarts()
        {
            return _cartRepository.GetCart();
        }

        /// <summary>
        /// Adds a product to the shopping cart with specified quantity
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <param name="quantity">Quantity of product to add</param>
        public void AddProductToCart(Product product, int quantity)
        {
            var Cart = new Cart
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                Quantity = quantity
            };

            _cartRepository.AddToCart(Cart);
        }

        /// <summary>
        /// Removes a specific item from the shopping cart
        /// </summary>
        /// <param name="itemId">ID of the item to remove</param>
        public void DeleteCartItem(int itemId)
        {
            var items = _cartRepository.GetCart();
            items.RemoveAll(item => item.ProductId == itemId);
            _cartRepository.SaveCart(items);
        }

        /// <summary>
        /// Retrieves all completed orders
        /// Returns a list of lists where each inner list represents one order
        /// </summary>
        public List<List<Cart>> GetOrders()
        {
            return _cartRepository.GetOrders();
        }

        /// <summary>
        /// Processes checkout for selected items in cart
        /// Moves selected items to orders and removes them from active cart
        /// </summary>
        /// <param name="selectedProductIds">Array of product IDs to checkout</param>
        public void Checkout(int[] selectedProductIds)
        {
            var cartItems = _cartRepository.GetCart();
            var selectedItems = cartItems.Where(c => selectedProductIds.Contains(c.ProductId)).ToList();

            if (selectedItems.Count == 0) return;

            // Get existing orders as a list of lists
            var orderList = _cartRepository.GetOrders();

            // Add selected items as a new separate order list
            orderList.Add(selectedItems);
            _cartRepository.SaveOrders(orderList);

            // Remove selected items from cart
            cartItems.RemoveAll(c => selectedProductIds.Contains(c.ProductId));
            _cartRepository.SaveCart(cartItems);
        }

        /// <summary>
        /// Searches for items in the current cart by product name
        /// Case-insensitive search
        /// </summary>
        /// <param name="searchTerm">Term to search for in product names</param>
        public List<Cart> SearchCart(string searchTerm)
        {
            var carts = _cartRepository.GetCart();
            return carts.Where(
                cart => cart.ProductName.ToLower().Contains(searchTerm.ToLower())
            ).ToList();
        }

        /// <summary>
        /// Searches through all orders for products matching search term
        /// Returns matching items grouped by their original orders
        /// Case-insensitive search
        /// </summary>
        /// <param name="searchTerm">Term to search for in product names</param>
        public List<List<Cart>> SearchOrders(string searchTerm)
        {
            var orders = _cartRepository.GetOrders();
            return orders
            .Select(orderList => orderList
                .Where(cart => cart.ProductName.ToLower().Contains(searchTerm.ToLower()))
                .ToList()
            )
            .Where(filteredList => filteredList.Count > 0) // Remove empty lists
            .ToList();
        }
    }
}
