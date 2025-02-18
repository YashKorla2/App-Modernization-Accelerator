using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Repositories;

namespace Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public List<Cart> GetCarts()
        {
            return _cartRepository.GetCart();
        }

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

        public void DeleteCartItem(int itemId)
        {
            var items = _cartRepository.GetCart();
            items.RemoveAll(item => item.ProductId == itemId);
            _cartRepository.SaveCart(items);
        }

        public List<List<Cart>> GetOrders()
        {
            return _cartRepository.GetOrders();
        }

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

        public List<Cart> SearchCart(string searchTerm)
        {
            var carts = _cartRepository.GetCart();
            return carts.Where(
                cart => cart.ProductName.ToLower().Contains(searchTerm.ToLower())
            ).ToList();
        }

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