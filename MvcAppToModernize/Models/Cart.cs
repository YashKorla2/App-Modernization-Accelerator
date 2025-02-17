using System;

namespace Models 
{
    public class Cart
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal TotalPrice => Quantity * ProductPrice;
    }
}