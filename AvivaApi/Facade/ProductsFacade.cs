using AvivaApi.Data;
using AvivaLibrary.Models;

namespace AvivaApi.Facade
{
    public class ProductsFacade(ProductRepository rep) : IProductsFacade
    {
        readonly ProductRepository repository = rep;

        public async Task<List<Product>> GetProductsAsync()
        {
            List<Product> products = await repository.GetAllAsync();
            return products;
        }
    }
}
