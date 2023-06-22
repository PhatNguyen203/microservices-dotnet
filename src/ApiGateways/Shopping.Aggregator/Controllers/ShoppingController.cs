using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly IBasketService basketService;
        private readonly IOrderService orderService;
        private readonly ICatalogService catalogService;

        public ShoppingController(IBasketService basketService, IOrderService orderService, ICatalogService catalogService)
        {
            this.basketService = basketService;
            this.orderService = orderService;
            this.catalogService = catalogService;
        }

        [HttpGet("{username}", Name ="GetShopping")]
        public async Task<ActionResult<ShoppingModel>>Get(string username)
        {
            // get basket with username
            // iterate basket items and consume products with basket item productId member
            // map product related members into basketitem dto with extended columns
            // consume ordering microservices in order to retrieve order list
            // return root ShoppngModel dto class which including all responses
            var basket = await basketService.GetBasketByUsername(username);
            foreach(var item in basket.Items)
            {
                var product = await catalogService.GetCatalogByName(item.BrandName);

                //set additional product fields onto basket item
                item.BrandName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }
            var orderInfo = await orderService.GetOrderInfoByUsername(username);
            var shoppingModel = new ShoppingModel()
            {
                Username = username,
                Basket = basket,
                Orders = orderInfo
            };

            return Ok(shoppingModel);
        }
    }
}
