using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient httpClient;

        public CatalogService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            var response = await httpClient.GetAsync($"/api/Catalog/GetProductByCategory/{category}");
            var catalogs = await response.ReadContentAs<List<CatalogModel>>();
            return catalogs;
        }

        public async Task<CatalogModel> GetCatalogById(string id)
        {
            var response = await httpClient.GetAsync($"/api/Catalog/{id}");
            var catalog = await response.ReadContentAs<CatalogModel>();
            return catalog;
        }
        public async Task<CatalogModel> GetCatalogByName(string name)
        {
            var response = await httpClient.GetAsync($"/api/Catalog/GetProductByName/{name}");
            var catalog = await response.ReadContentAs<CatalogModel>();
            return catalog;
        }
        public async Task<IEnumerable<CatalogModel>> GetCatalogs()
        {
            var response = await httpClient.GetAsync($"/api/Catalog");
            var catalogs = await response.ReadContentAs<List<CatalogModel>>();
            return catalogs;
        }
    }
}
