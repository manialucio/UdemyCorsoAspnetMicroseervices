using System;
using System.Linq;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CartModel : PageModel
    {
        private readonly IBasketService _cartRepository;

        public CartModel(IBasketService cartRepository)
        {
            _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        }

        public BasketModel Cart { get; set; } = new BasketModel();        

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _cartRepository.GetBasket("Lucio");            

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
            var basket = await _cartRepository.GetBasket("Lucio");
            var item = basket.Items.Single( x=> x.ProductId == productId );
            basket.Items.Remove( item );
            await _cartRepository.UpdateBasket(basket);
            return RedirectToPage();
        }
    }
}