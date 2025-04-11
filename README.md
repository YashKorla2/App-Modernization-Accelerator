# App-Modernization-Accelerator

An e-commerce application built with ASP.NET MVC demonstrating modern application architecture patterns.

## Project Structure

- **Models**: Contains domain models for Products, Cart, and Orders
- **Controllers**: Handles HTTP requests for Products, Cart, and Orders
- **Services**: Business logic layer with interfaces and implementations
- **Repositories**: Data access layer for product and cart operations
- **ExternalAPI**: External service integrations

## Features

- Product catalog with categories (tech, household, fashion, food)
- Shopping cart functionality
- Order processing and management
- JSON-based data storage

## Technical Stack

- ASP.NET MVC 5.0.0
- Unity 4.0.1 for dependency injection
- AWS SDK for .NET
- Newtonsoft.Json 5.0.4

## Data Storage

The application uses JSON files for data storage:
- `products.json`: Product catalog with details like name, price, category, and inventory
- `cart.json`: Current shopping cart state
- `order.json`: Order history and details

## Dependencies

Key packages:
- AWSSDK.Core 3.7.402.4
- AWSSDK.S3 3.7.415.3
- Unity.Mvc 4.0.1
- Microsoft.AspNet.Mvc 5.0.0