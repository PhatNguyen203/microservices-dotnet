using Discount.Grpc.Protos;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.GrpcServices
{
    public class DiscountClientService 
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient discountProtoService;

        public DiscountClientService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            this.discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
        }
        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest() { ProductName = productName };
            return await discountProtoService.GetDiscountAsync(discountRequest);
        }
    }
}
