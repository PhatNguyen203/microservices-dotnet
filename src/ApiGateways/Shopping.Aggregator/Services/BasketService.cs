using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient httpClient;

        public BasketService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<BasketModel> GetBasketByUsername(string username)
        {
            var response = await httpClient.GetAsync($"/api/Basket/username?username={username}");
            var basket = await response.ReadContentAs<BasketModel>();
            return basket;
        }
    }
}
