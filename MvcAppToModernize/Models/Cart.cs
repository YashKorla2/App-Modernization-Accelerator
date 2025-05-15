// This Cart class is used in:
// - CartController.cs: For handling HTTP requests related to shopping cart operations
// - CartService.cs: Contains business logic for cart management
// - CartRepository.cs: Handles data access operations for cart items

using System;

namespace Models 
{
    /// <summary>
    /// Represents a shopping cart item in an e-commerce system
    /// </summary>
    /// <remarks>
    /// Usage examples:
    /// - Create new cart item: var cartItem = new Cart { ProductId = 1, ProductName = "Book", Quantity = 2, ProductPrice = 29.99m };
    /// - Access total price: decimal total = cartItem.TotalPrice;
    /// - Update quantity: cartItem.Quantity += 1;
    /// 
    /// Common scenarios:
    /// - Shopping cart management
    /// - Order processing 
    /// - Cart item manipulation
    /// </remarks>
    public class Cart
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product in the cart
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name/description of the product
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the number of items of this product in cart
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product
        /// </summary>
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// Gets the calculated total price based on quantity and unit price.
        /// Auto-calculated property that updates when Quantity or ProductPrice changes.
        /// </summary>
        public decimal TotalPrice => Quantity * ProductPrice;
    }
}
