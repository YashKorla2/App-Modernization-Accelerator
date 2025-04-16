using System;

namespace Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
        public double ProductRating { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductAvailableQuantity { get; set; }
        public decimal ProductDiscount { get; set; }
    }
}