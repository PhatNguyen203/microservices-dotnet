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
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;

        public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.emailService = emailService;
        }
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var existingOrder = await orderRepository.GetByIdAsync(request.Id);

            //mapping re
            mapper.Map(request, existingOrder, typeof(UpdateOrderCommand), typeof(Order));

            await orderRepository.UpdateAsync(existingOrder);
            await SendMail(existingOrder.Id);
            return Unit.Value;
        }

        private async Task SendMail(int orderId)
        {
            var email = new Email() { To = "test@gmail.com", Body = $"Order {orderId} was updated.", Subject = $"Order {orderId} was updated" };

            try
            {
                await emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Order {orderId} failed due to an error with the mail service: {ex.Message}");
            }
        }
    }
}
