using AvivaLibrary.Models;
using AvivaUI.Proxies;

namespace AvivaUI.Services
{
    public class ProductServices(IProductsProxy productsProxy) : IProductServices
    {
        IProductsProxy proxy = productsProxy;

        public async Task<List<Product>> GetAllProductsAsync()
        {

            List<Product> products = await proxy.GetAllProductsAsync();
            return products;


        }
    }
}
