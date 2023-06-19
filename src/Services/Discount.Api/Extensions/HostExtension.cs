using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Api.Extensions
{
    public static class HostExtension
    {
        public static IHost MigratePostgresDatabase<T>(this IHost host, int? retry = 0 )
        {
            int retryAvalaibility = retry.Value;

            var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            var config = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<T>>();

            try
            {
                logger.LogInformation("Connecting to Postgres database...");

                using var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSettings:PostgresConnectionString"));
                connection.Open();
                logger.LogInformation("Database connected");

                using var command = new NpgsqlCommand() { Connection = connection };

                logger.LogInformation("Seeding data...");

                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE Coupon(
                        Id SERIAL PRIMARY KEY NOT NULL,
                        ProductName Varchar(50) NOT NULL,
                        Description Text,
                        Amount Numeric(6,2))";
                command.ExecuteNonQuery();

                command.CommandText = @"INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 99.99)";
                command.ExecuteNonQuery();

                command.CommandText = @"INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 9', 'Samsung Discount', 99.99)";
                command.ExecuteNonQuery();

                logger.LogInformation("Seeded Data");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                if(retryAvalaibility < 5)
                {
                    retryAvalaibility++;
                    MigratePostgresDatabase<T>(host, retryAvalaibility);
                }
            }

            return host;
        }
    }
}
