using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
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
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMapper mapper;
        private readonly IEmailService emailService;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.emailService = emailService;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var existingOrder = await orderRepository.GetByIdAsync(request.Id);
            if(existingOrder == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }
            int orderId = existingOrder.Id;
            await orderRepository.DeleteAsync(existingOrder);
            await SendMail(orderId);
            return Unit.Value;
        }

        private async Task SendMail(int orderId)
        {
            var email = new Email() { To = "test@gmail.com", Body = $"Order {orderId} was deleted.", Subject = $"Order {orderId} was deleted" };

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
