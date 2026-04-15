using AvivaLibrary.Models;

namespace AvivaApi.Facade
{
    public interface IProductsFacade
    {
        Task<List<Product>> GetProductsAsync();
    }
}