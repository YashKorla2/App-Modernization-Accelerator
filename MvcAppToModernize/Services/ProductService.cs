using System.Collections.Generic;
using System.Linq;
using Models;
using Repositories;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public Product GetProductById(int productId)
        {
            return _productRepository.GetProductById(productId);
        }

        public void AddProduct(Product product)
        {
            var products = _productRepository.GetAllProducts();
            int maxId = products.Any() ? products.Max(p => p.ProductId) : 0;
            product.ProductId = maxId + 1;
            products.Add(product);
            _productRepository.SaveProducts(products);
        }

        public void UpdateProduct(Product updatedProduct)
        {
            var products = _productRepository.GetAllProducts();
            int index = products.FindIndex(p => p.ProductId == updatedProduct.ProductId);
            if (index != -1)
            {
                products[index] = updatedProduct;
                _productRepository.SaveProducts(products);
            }
        }

        public void DeleteProduct(int productId)
        {
            var products = _productRepository.GetAllProducts();
            products.RemoveAll(p => p.ProductId == productId);
            _productRepository.SaveProducts(products);
        }

        public List<Product> SearchProducts(string searchTerm)
        {
            var products = _productRepository.GetAllProducts();
            return products.Where(
                p => p.ProductName.ToLower().Contains(searchTerm.ToLower()) || 
                p.ProductDescription.ToLower().Contains(searchTerm.ToLower()) ||
                p.ProductCategory.ToLower().Contains(searchTerm.ToLower())
            ).ToList();
        }
    }
}
