// This Product class is used as a data model across multiple components:
// - ProductController: Handles HTTP requests for product CRUD operations
// - ProductRepository: Manages product data persistence in the database
// - ProductService: Contains business logic for product operations
// - ProductViewModel: Used to display product data in views

// Common usage patterns:
// - Creating new products: new Product() with required properties
// - Retrieving products: Database queries return Product objects
// - Product listings: IEnumerable<Product> for displaying multiple products
// - Shopping cart: Product objects referenced in cart items
// - Order processing: Product quantities updated during checkout

using System;

namespace Models
{
    public class Product
    {
        // Unique identifier for the product
public class Product
    {
        // Unique identifier for the product
        public int Id { get; set; }

        // Display name shown in product listings and details
        public string Name { get; set; }

        // Detailed product information shown on product details page
        public string Description { get; set; }

        // Used for product filtering and navigation
        public string Category { get; set; }

        // Average customer rating (0-5 scale)
        public double Rating { get; set; }

        // Current selling price
        public decimal Price { get; set; }

        // Current stock level, updated during order processing
        public int AvailableQuantity { get; set; }

        // Applied discount amount or percentage
        public decimal Discount { get; set; }
    }
}

        // Display name shown in product listings and details
        public string ProductName { get; set; }

        // Detailed product information shown on product details page
        public string ProductDescription { get; set; }

        // Used for product filtering and navigation
        public string ProductCategory { get; set; }

        // Average customer rating (0-5 scale)
        public double ProductRating { get; set; }

        // Current selling price
        public decimal ProductPrice { get; set; }

        // Current stock level, updated during order processing
        public int ProductAvailableQuantity { get; set; }

        // Applied discount amount or percentage
        public decimal ProductDiscount { get; set; }
    }
}
