using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Models;

namespace Repositories
{
    /// <summary>
    /// Repository class that handles shopping cart and order data persistence using JSON files
    /// </summary>
    public class CartRepository : ICartRepository
    {
        // File paths for storing cart and order data in JSON format
        private readonly string _cartFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "cart.json");
        private readonly string _orderFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "order.json");

        /// <summary>
        /// Retrieves the current shopping cart from the JSON file
        /// Returns empty list if file doesn't exist
        /// </summary>
        public List<Cart> GetCart()
        {
            if (!File.Exists(_cartFilePath))
                return new List<Cart>();

            string json = File.ReadAllText(_cartFilePath);
            return JsonConvert.DeserializeObject<List<Cart>>(json) ?? new List<Cart>();
        }

        /// <summary>
        /// Saves the current shopping cart state to JSON file
        /// </summary>
        /// <param name="cart">List of cart items to save</param>
        public void SaveCart(List<Cart> cart)
        {
            string json = JsonConvert.SerializeObject(cart, Formatting.Indented);
            File.WriteAllText(_cartFilePath, json);
        }

        /// <summary>
        /// Adds an item to the shopping cart
        /// If item already exists, increases its quantity
        /// If item is new, adds it to the cart
        /// </summary>
        /// <param name="item">Cart item to add</param>
        public void AddToCart(Cart item)
        {
            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(p => p.ProductId == item.ProductId);

            if (existingItem != null)
                existingItem.Quantity += item.Quantity;
            else
                cart.Add(item);

            SaveCart(cart);
        }

        /// <summary>
        /// Retrieves all orders history from the JSON file
        /// Returns empty list if file doesn't exist
        /// </summary>
/// Retrieves all orders history from the JSON file
        /// Returns empty list if file doesn't exist
        /// </summary>
        public List<List<Cart>> GetOrders()
        {
            try
            {
                if (!File.Exists(_orderFilePath))
                    return new List<List<Cart>>();

                string json = File.ReadAllText(_orderFilePath);
                return JsonConvert.DeserializeObject<List<List<Cart>>>(json) ?? new List<List<Cart>>();
            }
            catch (IOException ex)
            {
                // TODO: Log the exception
                return new List<List<Cart>>();
            }
            catch (JsonException ex)
            {
                // TODO: Log the exception
                return new List<List<Cart>>();
            }
        }

        /// <summary>
        /// Saves the orders history to JSON file
        /// </summary>
        /// <param name="orders">List of orders to save</param>
        public void SaveOrders(List<List<Cart>> orders)
        {
            try
            {
                string json = JsonConvert.SerializeObject(orders, Formatting.Indented);
                File.WriteAllText(_orderFilePath, json);
            }
            catch (IOException ex)
            {
                // TODO: Log the exception
            }
            catch (JsonException ex)
            {
                // TODO: Log the exception
            }
        }

    }

}
        {
            if (!File.Exists(_orderFilePath))
                return new List<List<Cart>>();

            string json = File.ReadAllText(_orderFilePath);
            return JsonConvert.DeserializeObject<List<List<Cart>>>(json) ?? new List<List<Cart>>();
        }

        /// <summary>
        /// Saves the orders history to JSON file
        /// </summary>
        /// <param name="orders">List of orders to save</param>
        public void SaveOrders(List<List<Cart>> orders)
        {
            string json = JsonConvert.SerializeObject(orders, Formatting.Indented);
            File.WriteAllText(_orderFilePath, json);
        }

    }

}
