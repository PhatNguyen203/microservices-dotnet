using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Shopping.Aggregator.Extensions;


namespace Shopping.Aggregator.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient httpClient;

        public OrderService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<OrderInfoModel>> GetOrderInfoByUsername(string username)
        {
            var response = await httpClient.GetAsync($"api/Order/{username}");

            var orderInfo = await response.ReadContentAs<List<OrderInfoModel>>();

            return orderInfo;
        }
    }
}
