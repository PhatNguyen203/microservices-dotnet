using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Features.Orders.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Api.EventBusConsumer
{
    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private readonly ILogger<BasketCheckoutConsumer> logger;

        public BasketCheckoutConsumer(IMediator mediator, IMapper mapper, ILogger<BasketCheckoutConsumer> logger)
        {
            this.mediator = mediator;
            this.mapper = mapper;
            this.logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var command = mapper.Map<CheckoutOrderCommand>(context.Message);
            var result =  await mediator.Send(command);

            logger.LogInformation("BasketCheckoutEvent consumed successfully. Created Order Id : {result}", result);
        }
    }
}
