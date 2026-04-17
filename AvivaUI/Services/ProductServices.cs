using AvivaLibrary.Models;
using AvivaUI.Proxies;

namespace AvivaUI.Services
{
    public class ProductServices(IProductsProxy productsProxy) : IProductServices
    {
        readonly IProductsProxy proxy = productsProxy;

        public async Task<string> CheckConnectionAliveAsync()
        {
            string connection = await proxy.CheckConnectionAliveAsync();
            return connection;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {

            List<Product> products = await proxy.GetAllProductsAsync();
            return products;


        }
    }
}
