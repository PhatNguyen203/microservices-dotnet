using Catalog.Api.Entities;
using Catalog.Api.Repositories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CatalogController : ControllerBase
	{
		private readonly IProductRepository repository;
		private readonly ILogger<CatalogController> logger;

		public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
		{
			this.repository = repository;
			this.logger = logger;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await repository.GetProducts();
			return Ok(products);
		}

		[HttpGet("{id:length(24)}", Name = "GetProductById")]
		public async Task<ActionResult<Product>> GetProductById(string id)
		{
			var product = await repository.GetProductById(id);

			if (product == null)
			{
				logger.LogError($"Product with id: {id}, not found.");
				return NotFound();
			}

			return Ok(product);
		}

		[Route("[action]/{category}", Name = "GetProductByCategory")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
		{
			var products = await repository.GetProductByCategory(category);
			return Ok(products);
		}

		[Route("[action]/{name}", Name = "GetProductByName")]
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProductByName(string name)
		{
			var items = await repository.GetProductByName(name);
			if (items == null)
			{
				logger.LogError($"Products with name: {name} not found.");
				return NotFound();
			}
			return Ok(items);
		}

		[HttpPost]
		public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
		{
			await repository.CreateProduct(product);

			return CreatedAtRoute("GetProductById", new { id = product.Id }, product);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateProduct([FromBody] Product product)
		{
			return Ok(await repository.UpdateProduct(product));
		}

		[HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
		public async Task<IActionResult> DeleteProductById(string id)
		{
			return Ok(await repository.DeleteProduct(id));
		}
	}
}
