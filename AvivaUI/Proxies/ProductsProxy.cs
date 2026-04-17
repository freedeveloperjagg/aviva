using AvivaLibrary.Models;

namespace AvivaUI.Proxies
{
    public class ProductsProxy(IHttpClientFactory factory) : IProductsProxy
    {
        private readonly IHttpClientFactory httpClientFactory = factory;

        public async Task<string> CheckConnectionAliveAsync()
        {
            var client = httpClientFactory.CreateClient("AvivaApi");

            var response = await client.GetAsync("/api/products/Alive");

            if (response.IsSuccessStatusCode)
            {
                var alive = await response.Content.ReadAsStringAsync();
                return alive;
            }
            else
            {
                // Handle error response as needed
                throw new Exception($"Failed to Connect with API: {response.ReasonPhrase}");
            }
        }

        /// <summary>
        /// Get Products from API
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var client = httpClientFactory.CreateClient("AvivaApi");

            var response = await client.GetAsync("/api/products");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<Product>>();
                return products ?? [];
            }
            else
            {
                // Handle error response as needed
                throw new Exception($"Failed to retrieve products: {response.ReasonPhrase}");
            }
        }
    }
}
