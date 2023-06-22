using Shopping.Aggregator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services.Contracts
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasketByUsername(string username);
    }
}
