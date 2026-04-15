using AvivaLibrary.Models;

namespace AvivaApi.Bo
{
    public interface IProductsBo
    {
        Task<List<Product>> GetProductsAsync();
    }
}