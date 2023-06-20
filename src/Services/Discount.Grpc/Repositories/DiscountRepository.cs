using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Repositories.Contracts;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<bool> CreateCoupon(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:PostGresConnectionString"));

            string insertQuery = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES(@ProductName, @Description, @Amount)";

            var affectedRow = await connection.ExecuteAsync(insertQuery,
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            if (affectedRow == 0)
                return false;
            return true;
        }

        public async Task<bool> DeleteCoupon(string productName)
        {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:PostGresConnectionString"));
            string deleteQuery = "DELETE FROM Coupon WHERE ProductName=@productName";

            var affectedRow = await connection.ExecuteAsync(deleteQuery, new { ProductName = productName });

            if (affectedRow == 0)
                return false;
            return true;
        }

        public async Task<Coupon> GetCouponDiscount(string productName)
        {
           var query = "SELECT * FROM Coupon WHERE ProductName = @ProductName";
           using(var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:PostGresConnectionString")))
           {
                var result = await connection.QueryFirstOrDefaultAsync<Coupon>(query, new { ProductName = productName });
                if (result == null)
                    return new Coupon()
                    {
                        ProductName = "No Discount",
                        Amount = 0,
                        Description = "No Discount Description"
                    };
                return result;
           }
        }

        public async Task<bool> UpdateCoupon(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:PostgresConnectionString"));
            string updateQuery = "UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id";

            var affectedRow = await connection.ExecuteAsync(updateQuery, 
                    new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            if (affectedRow == 0)
                return false;
            return true;
        }
    }
}
