using Catalog.API.Entities;
using Catalog.API.Model;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly OrderCommand _orderCommand;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger, IHttpClientFactory clientFactory, OrderCommand orderCommand)
        {
            _repository = repository;
            _logger = logger;
            _clientFactory = clientFactory;
            _orderCommand = orderCommand;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            _logger.LogInformation("Get all products method is invoked!");

            _logger.LogInformation("Before getting all the products");

            var products = await _orderCommand.RandomOrderAsync();

            _logger.LogInformation("After getting all the products");
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProductById")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _repository.GetProduct(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id} not found.");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        [Route("{action}/{category}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _repository.getProductByCategory(category);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _repository.Create(product);
            return CreatedAtRoute("GetProductById", new { id = product.Id}, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]

        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            var productInfo = await _repository.GetProduct(product.Id);
            if (productInfo == null)
                return NotFound();

            return Ok(await _repository.Update(product));
        }

        [HttpDelete ("{id:length(24)}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            var productInfo = await _repository.GetProduct(id);
            if (productInfo == null)
                return NotFound();

            return Ok(await _repository.Delete(id));
        }
    }
}
