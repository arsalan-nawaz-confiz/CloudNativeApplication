using Catalog.API.Entities;
using Catalog.API.Model;
using Catalog.API.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Steeltoe.CircuitBreaker.Hystrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.API.Services
{
    public class OrderCommand: HystrixCommand<List<Product>>
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<OrderCommand> _logger;

        public OrderCommand(IHystrixCommandOptions options, IProductRepository repository, ILogger<OrderCommand> logger) : base(options)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<List<Product>> RandomOrderAsync()
        {
            return await ExecuteAsync();
        }

        protected override async Task<List<Product>> RunAsync()
        {
            _logger.LogInformation("Got the orders successfully");
            var client = new HttpClient();
            var response = client.GetAsync("http://localhost:56810/order").Result;

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var orders = await JsonSerializer.DeserializeAsync<List<Order>>(responseStream);

            var products = await _repository.GetProducts();
            _logger.LogInformation("returning orders");
            return products.ToList();
        }

        protected override async Task<List<Product>> RunFallbackAsync()
        {
            _logger.LogError("request failed to get a success from orders");
            Product product = new Product();
            product.Id = "ABC123";
            product.Category = "Fallback Category";
            product.Name = "Fallback Product";
            product.Description = "Fallback Product Description";

            var products = new List<Product>();
            products.Add(product);

            _logger.LogInformation("returning fallback orders");
            return await Task.Run(() => products);
        }
    }
}
