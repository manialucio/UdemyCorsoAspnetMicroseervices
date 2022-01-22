using System;
using System.Threading.Tasks;
using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspnetRunBasics
{
    public class CheckOutModel : PageModel
    {
        private readonly IBasketService _cartService;
        private readonly IOrderService _orderService;

        public CheckOutModel(IBasketService cartRepository, IOrderService orderRepository)
        {
            _cartService = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
            _orderService = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        [BindProperty]
        public OrderResponseModel Order { get; set; }

        public BasketModel Cart { get; set; } = new BasketModel();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _cartService.GetBasket("Lucio");
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            Cart = await _cartService.GetBasket("Lucio");

            if (!ModelState.IsValid)
            {
                return Page();
            }
             
            return RedirectToPage("Confirmation", "OrderSubmitted");
        }       
    }
}