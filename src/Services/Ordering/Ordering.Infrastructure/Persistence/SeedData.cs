using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class SeedData 
    {
        public static async Task SeedAsync(OrderDbContext context, ILogger<SeedData> logger)
        {
            if (!context.Orders.Any())
            {
                context.Orders.AddRange(Seeding());
                await context.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderDbContext).Name);
            }
        }

        private static IEnumerable<Order> Seeding()
        {
            return new List<Order>
            {
                new Order() 
                {UserName = "swn", FirstName = "Phat", LastName = "Nguyen", EmailAddress = "test@gmail.com", AddressLine = "Houston, TX", Country = "USA", TotalPrice = 350 }
            };
        }
    }
}
