using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Repositories.Contracts
{
    public interface IBasketRepository
    {
        Task<Entities.Basket> GetBasket(string username);
        Task<Entities.Basket> UpdateBasket(Entities.Basket basket);
        Task DeleteBasket(string username);
    }
}
