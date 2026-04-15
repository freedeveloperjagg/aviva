using AvivaLibrary.Models;

namespace AvivaUI.Proxies
{
    public interface IProductsProxy
    {
        Task<List<Product>> GetAllProductsAsync();
    }
}