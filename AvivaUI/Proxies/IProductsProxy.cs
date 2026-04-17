using AvivaLibrary.Models;

namespace AvivaUI.Proxies
{
    public interface IProductsProxy
    {
        Task<string> CheckConnectionAliveAsync();
        Task<List<Product>> GetAllProductsAsync();
    }
}