using AvivaLibrary.Models;

namespace AvivaUI.Proxies
{
    public class ProductsProxy(IHttpClientFactory factory) : IProductsProxy
    {
        private readonly IHttpClientFactory httpClientFactory = factory;

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
                return products ?? new List<Product>();
            }
            else
            {
                // Handle error response as needed
                throw new Exception($"Failed to retrieve products: {response.ReasonPhrase}");
            }
        }
    }
}
