using AutoMapper;
using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories.Contracts;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IPublishEndpoint publishEndpoint;
        private readonly ILogger<BasketController> logger;
        private readonly IMapper mapper;

        public BasketController(IBasketRepository basketRepository, DiscountClientService discountClientService,
            IPublishEndpoint publishEndpoint, ILogger<BasketController> logger, IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.discountClientService = discountClientService;
            this.publishEndpoint = publishEndpoint;
            this.logger = logger;
            this.mapper = mapper;
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
                logger.LogInformation("Adding discount for {0}", item.BrandName);
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
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult>Checkout([FromBody] BasketCheckout basketCheckout)
        {
            // get existing basket with total price            
            // Set TotalPrice on basketCheckout eventMessage
            // send checkout event to rabbitmq
            // remove the basket
            var basket = await basketRepository.GetBasket(basketCheckout.UserName);

            if (basket == null)
                return BadRequest();

            var eventMsg = mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMsg.TotalPrice = basket.Total;
            await publishEndpoint.Publish<BasketCheckoutEvent>(eventMsg);

            await basketRepository.DeleteBasket(basket.Username);

            return Accepted();
        }
    }
}
