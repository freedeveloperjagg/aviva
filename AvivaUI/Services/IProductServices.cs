using AvivaLibrary.Models;

namespace AvivaUI.Services
{
    public interface IProductServices
    {
        Task<List<Product>> GetAllProductsAsync();
    }
}