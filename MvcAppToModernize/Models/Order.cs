// This Order class is used in:
// - OrderController.cs: For handling HTTP requests related to order operations
// - OrderService.cs: Contains business logic for order processing
// - OrderRepository.cs: Handles data access operations for orders

using System;

namespace Models 
{
    /// <summary>
    /// Represents an order in the system
    /// Used for order creation, processing and management
    /// Key integrations:
    /// - Maps to Orders table in database
    /// - Used in order API responses
    /// - Passed between service layer and data access layer
    /// </summary>
    public class Order
    {
        // Used for database operations and order tracking
        public int ProductId { get; set; }
        
        // Used in order summaries and reports
        public string ProductName { get; set; }
        
        // Used for inventory management and order fulfillment
        public int Quantity { get; set; }
        
        // Used for individual product pricing
        public decimal ProductPrice { get; set; }
        
        // Used for order totals and financial calculations
        public decimal TotalPrice => Quantity * ProductPrice;
    }
}
