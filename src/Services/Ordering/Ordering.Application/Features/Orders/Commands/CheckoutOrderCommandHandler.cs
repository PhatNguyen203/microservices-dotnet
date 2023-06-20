using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private IOrderRepository orderRepository;
        private IMapper mapper;
        private IEmailService emailService;

        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService)
        {
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = mapper.Map<Order>(request);
            var newOrder = await orderRepository.AddAsync(orderEntity);

            await SendMail(newOrder);

            return newOrder.Id;
        }

        private async Task SendMail(Order order)
        {
            var email = new Email() { To = "test@gmail.com", Body = $"Order was created.", Subject = "Order was created" };

            try
            {
                await emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
               Console.WriteLine($"Order {order.Id} failed due to an error with the mail service: {ex.Message}");
            }
        }
    }
}
