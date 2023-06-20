using MediatR;
using Ordering.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries
{
    public class GetOrderListByUsernameQuery : IRequest<List<OrderVm>>
    {
        public string Username { get; set; }
        public GetOrderListByUsernameQuery(string userName)
        {
            Username = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
