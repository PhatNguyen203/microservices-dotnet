using Shopping.Aggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services.Contracts
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderInfoModel>> GetOrderInfoByUsername(string username);
    }
}
