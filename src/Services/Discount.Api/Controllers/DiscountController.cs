using Discount.Api.Entities;
using Discount.Api.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository repository;

        public DiscountController(IDiscountRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("productName", Name = "GetDiscount")]
        public async Task<ActionResult<Coupon>>Get(string productName)
        {
            return Ok(await repository.GetCouponDiscount(productName));
        }
        [HttpPost]
        public async Task<ActionResult>Create([FromBody] Coupon coupon)
        {
            var result = await repository.CreateCoupon(coupon);
            if (!result)
                return BadRequest();
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }
        [HttpPut]
        public async Task<ActionResult>Update([FromBody] Coupon coupon)
        {
            var result = await repository.UpdateCoupon(coupon);
            if (!result)
                return BadRequest();
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }
        [HttpDelete("productName")]
        public async Task<ActionResult>Delete(string productName)
        {
            var result = await repository.DeleteCoupon(productName);
            if (!result)
                return BadRequest();
            return NoContent();
        }
    }
}
