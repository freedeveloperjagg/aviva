using AvivaApi.Facade;
using AvivaLibrary.Models;

namespace AvivaApi.Bo
{
    public class ProductsBo(IProductsFacade fac) : IProductsBo
    {
        readonly IProductsFacade facade = fac;

        public async Task<List<Product>> GetProductsAsync()
        {
            List<Product> products = await facade.GetProductsAsync();
            return products;
        }
    }
}
