using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Api.Extensions
{
    public static class HostExtension
    {
        public static IHost MigrateDatabase<TDbContext>(this IHost host, Action<TDbContext, IServiceProvider> seeder) where TDbContext : DbContext
        {

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<TDbContext>>();
                var context = services.GetRequiredService<TDbContext>();

                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TDbContext).Name);

                    var retry = Policy.Handle<SqlException>()
                          .WaitAndRetry(
                              retryCount: 5,
                              sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // 2,4,8,16,32 sc
                              onRetry: (exception, retryCount, context) =>
                              {
                                  logger.LogError($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
                              });

                    //if the sql server container is not created on run docker compose this
                    //migration can't fail for network related exception. The retry options for DbContext only 
                    //apply to transient exceptions                    
                    retry.Execute(() => InvokeSeeder(seeder, context, services));

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TDbContext).Name);

                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TDbContext).Name);
                }
            }

            return host;
        }

        private static void InvokeSeeder<TDbContext>(Action<TDbContext, IServiceProvider> seeder, TDbContext context, IServiceProvider services)
          where TDbContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
