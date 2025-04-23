// This class is used as a view model in the e-commerce application
// Used in ProductController.Index() to display product catalog with shopping cart status
// Used in Views/Product/Index.cshtml to render product listings and cart count
// Products list holds the catalog items to be displayed
// CartItemCount tracks number of items in user's shopping cart for header display
using System.Collections.Generic;

namespace Models
{
    public class ProductViewModel 
    {
        public List<Product> Products { get; set; }
        public int CartItemCount { get; set; }
    }
}
