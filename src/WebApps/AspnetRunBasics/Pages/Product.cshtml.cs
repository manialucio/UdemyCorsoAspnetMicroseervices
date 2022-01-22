using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class ProductModel : PageModel
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;


        public ProductModel(ICatalogService productRepository, IBasketService cartRepository)
        {
            _catalogService = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _basketService = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public IEnumerable<string> CategoryList { get; set; } = new List<string>();
        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();


        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            if (!String.IsNullOrEmpty(categoryName))
            {
                ProductList = await _catalogService.GetCatalogByCategory(categoryName);
                SelectedCategory = CategoryList.FirstOrDefault();
            }
            else
            {
                ProductList = await _catalogService.GetCatalog();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });
            var product = await _catalogService.GetCatalog(productId);
            var basket = await _basketService.GetBasket("Lucio");
            basket.Items.Add(new BasketItemModel()
            {
                ProductId = productId,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = 1,
                Color = "Black"
            }
            );
            var basketUpdated = await _basketService.UpdateBasket(basket);
            return Page();
        }
    }
}