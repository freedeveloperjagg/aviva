using AvivaLibrary.Models;

namespace AvivaUI.Services
{
    public interface IProductServices
    {
        /// <summary>
        /// Check connection alive
        /// </summary>
        /// <returns></returns>
        Task<string> CheckConnectionAliveAsync();

        /// <summary>
        /// Get all the products
        /// </summary>
        /// <returns></returns>
        Task<List<Product>> GetAllProductsAsync();
    }
}