using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Models;

namespace Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly string _cartFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "cart.json");
        private readonly string _orderFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "order.json");

        public List<Cart> GetCart()
        {
            if (!File.Exists(_cartFilePath))
                return new List<Cart>();

            string json = File.ReadAllText(_cartFilePath);
            return JsonConvert.DeserializeObject<List<Cart>>(json) ?? new List<Cart>();
        }

        public void SaveCart(List<Cart> cart)
        {
            string json = JsonConvert.SerializeObject(cart, Formatting.Indented);
            File.WriteAllText(_cartFilePath, json);
        }

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

        public List<List<Cart>> GetOrders()
        {
            if (!File.Exists(_orderFilePath))
                return new List<List<Cart>>();

            string json = File.ReadAllText(_orderFilePath);
            return JsonConvert.DeserializeObject<List<List<Cart>>>(json) ?? new List<List<Cart>>();
        }

        public void SaveOrders(List<List<Cart>> orders)
        {
            string json = JsonConvert.SerializeObject(orders, Formatting.Indented);
            File.WriteAllText(_orderFilePath, json);
        }

    }

}