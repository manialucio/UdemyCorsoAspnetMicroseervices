using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public IndexModel(ICatalogService productRepository, IBasketService cartRepository)
        {
            _catalogService = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _basketService = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public IEnumerable<CatalogModel> ProductList { get; set; } = new List<CatalogModel>();

        public async Task<IActionResult> OnGetAsync()
        {
            ProductList = await _catalogService.GetCatalog();
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(string productId)
        {
            //if (!User.Identity.IsAuthenticated)
            //    return RedirectToPage("./Account/Login", new { area = "Identity" });
            var product = await _catalogService.GetCatalog(productId);
            var basket = await _basketService.GetBasket("Lucio");
            basket.Items.Add( new BasketItemModel() { ProductId = productId,
            ProductName = product.Name,
            Price = product.Price,
            Quantity = 1,
            Color="Black"}
            );
            var basketUpdated = await _basketService.UpdateBasket(basket);
            return RedirectToPage("Cart");
        }
    }
}
