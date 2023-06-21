using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Api.Profiles
{
    public class OrderingMapping : Profile
    {
        public OrderingMapping()
        {
            CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
