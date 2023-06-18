using Catalog.Api.Data;
using Catalog.Api.Entities;
using Catalog.Api.Repositories.Contracts;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ICatalogContext context;

		public ProductRepository(ICatalogContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IEnumerable<Product>> GetProducts()
		{
			return await context.Products.Find(p => true).ToListAsync();
		}

		public async Task<Product> GetProductById(string id)
		{
			return await context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Category, categoryName);
			return await context.Products.Find(filter).ToListAsync();
		}

		public async Task<IEnumerable<Product>> GetProductByName(string name)
		{
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);

			return await context.Products.Find(filter).ToListAsync();
		}

		public async Task CreateProduct(Product product)
		{
			await context.Products.InsertOneAsync(product);
		}

		public async Task<bool> DeleteProduct(string id)
		{
			//apply filter to find product by id
			FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
			//DeleteResult result = await context.Products.DeleteOneAsync<Product>(p => p.Id == id);
			DeleteResult result = await context.Products.DeleteOneAsync(filter);
			return result.IsAcknowledged && result.DeletedCount > 0;
		}
		public async Task<bool> UpdateProduct(Product product)
		{
			var updatedProduct = await context.Products.ReplaceOneAsync(g => g.Id == product.Id, product);
			return updatedProduct.IsAcknowledged && updatedProduct.ModifiedCount > 0;
		}
	}
}
