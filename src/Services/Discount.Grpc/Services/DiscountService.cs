using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories.Contracts;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository discountRepository;
        private readonly IMapper mapper;

        public DiscountService(IDiscountRepository discountRepository, IMapper mapper)
        {
            this.discountRepository = discountRepository;
            this.mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await discountRepository.GetCouponDiscount(request.ProductName);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"{request.ProductName} is Not Found"));
            }
            var couponModel = mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var newCoupon = mapper.Map<Coupon>(request.Coupon);
            var result = await discountRepository.CreateCoupon(newCoupon);
            if(result)
                return request.Coupon;
            return null;
        }
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = mapper.Map<Coupon>(request.Coupon);
            var result = await discountRepository.UpdateCoupon(coupon);
            if (result)
                return request.Coupon;
            return null;
        }
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var result = await discountRepository.DeleteCoupon(request.ProductName);
           
            return new DeleteDiscountResponse { Success = result };
        }
    }
}
