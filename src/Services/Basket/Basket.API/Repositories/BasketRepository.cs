using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task DeleteBasket(string userName)
        {
             await _cache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            var basket = await _cache.GetStringAsync(userName);
            if (basket == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException("basket");
            }
            else
            {
                await _cache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
                return await GetBasket(basket.UserName);
            }
         }
    }
}
