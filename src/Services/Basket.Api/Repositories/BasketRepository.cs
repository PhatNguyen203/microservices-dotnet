using Basket.Api.Repositories.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            this.redisCache = redisCache;
        }
        public async Task DeleteBasket(string username)
        {
            await redisCache.RemoveAsync(username);
        }

        public async Task<Entities.Basket> GetBasket(string username)
        {
            var basket = await redisCache.GetStringAsync(username);
            if(string.IsNullOrEmpty(basket))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Entities.Basket>(basket);
        }

        public async Task<Entities.Basket> UpdateBasket(Entities.Basket basket)
        {
            await redisCache.SetStringAsync(basket.Username, JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.Username);
        }
    }
}
