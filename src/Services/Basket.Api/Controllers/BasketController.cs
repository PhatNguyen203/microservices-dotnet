using Basket.Api.GrpcServices;
using Basket.Api.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository basketRepository;
        private readonly DiscountClientService discountClientService;

        public BasketController(IBasketRepository basketRepository, DiscountClientService discountClientService)
        {
            this.basketRepository = basketRepository;
            this.discountClientService = discountClientService;
        }

        [HttpGet("username")]
        public async Task<ActionResult<Entities.Basket>>Get(string username)
        {
            var basket = await basketRepository.GetBasket(username);
            if (basket == null)
                return NotFound();
            return Ok(basket);
        }
        [HttpPost]
        public async Task<ActionResult<Entities.Basket>>UpdateBasket([FromBody] Entities.Basket basket)
        {
            //Comunicate with discount gprc to get the discount for items
            foreach (var item in basket.Items)
            {
                var coupon = await discountClientService.GetDiscount(item.BrandName);
                item.Price -= (decimal)coupon.Amount;
            }
            return Ok(await basketRepository.UpdateBasket(basket));
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteBasket(string username)
        {
            await basketRepository.DeleteBasket(username);
            return Ok();
        }
    }
}
