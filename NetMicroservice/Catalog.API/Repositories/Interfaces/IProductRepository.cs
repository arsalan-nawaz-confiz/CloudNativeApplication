using Catalog.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> getProductByName(string name);
        Task<IEnumerable<Product>> getProductByCategory(string category);

        Task Create(Product product);

        Task<bool> Update(Product product);
        Task<bool> Delete(string id);

    }
}
