using AvivaApi.Proxy.Models;
using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
using System.Text;
using System.Text.Json;

namespace AvivaApi.Proxy
{
    public class PagaFacilProxy(
        IHttpClientFactory xfactory,
        CConfig xconfig) : IProxy
    {
        private readonly IHttpClientFactory factory = xfactory;
        private readonly CConfig cconfig = xconfig;
        private readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        // All in upper cases
        private const string ProviderName = "PAGAFACIL";

        public async Task<OrderCreated> CreateOrderAsync(OrderPago orderPago)
        {
            // Create the payload
            string json = JsonSerializer.Serialize(orderPago);
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = HttpCreateClient(payload);          

            var result = await client.PostAsync("Order", payload);
            string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            {
                Models.PagaFacilResponse? order = JsonSerializer.Deserialize<Models.PagaFacilResponse>(content, options);
                if (order != null)
                {
                    return order.ConvertPago(ProviderName);
                }

                throw new ApplicationException($"Order Created Returned null. something is very wrong in proxy in {ProviderName}:{client.BaseAddress}.");

            }
            throw new ApplicationException($"Error reading the proxy in {client.BaseAddress}Order, Error: {content}");
        }

        public async Task<OrderCreated?> GetOrderAsync(string orderProviderId)
        {
            var prov = cconfig.ProveedoresSettings.SingleOrDefault(x => x.Nombre.ToUpperInvariant() == ProviderName);
            if (prov == null) { throw new ArgumentException($"Provider Unknow when try to get order Id {orderProviderId}"); }
            
            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            var httpResponse = await client.GetAsync("Order/orderProviderId");
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                PagaFacilResponse? order = JsonSerializer.Deserialize<PagaFacilResponse>(content, options);
                if (order == null) return null;
                OrderCreated ord = order.ConvertPago(ProviderName);
                return ord;
            }
            throw new ApplicationException($"Access the Provider {prov.Nombre} fail with: {content}");
        }

        /// <summary>
        /// NOTE: This is not more used. informstion are getting from the DB
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public async Task<List<OrderCreated>> GetOrdersAsync()
        {
            var prov = cconfig.ProveedoresSettings.SingleOrDefault(x => x.Nombre.ToUpperInvariant() == ProviderName);
            if (prov == null) { throw new ArgumentException($"Provider Unknow when try to get all the orders"); }

            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            var httpResponse = await client.GetAsync("Order");
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                List<PagaFacilResponse>? orders = JsonSerializer.Deserialize<List<PagaFacilResponse>>(content, options);
                if (orders == null) return [];
                List<OrderCreated> ord = [];
                foreach (var orderItem in orders) 
                { 
                    ord.Add(orderItem.ConvertPago(ProviderName));
                }                
                return ord;

            }
            throw new ApplicationException($"Access the Provider {prov.Nombre} fail with: {content}");
        }

        public async Task PayOrderAsync(string orderProviderId)
        {
            // Get the provider by Name
            ProveedorSetting? prov = cconfig.ProveedoresSettings.FirstOrDefault(x => x.Nombre == ProviderName);
            if (prov == null)
            {
                throw new ApplicationException($"Provider unknow for {orderProviderId} ");
            }

            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            HttpResponseMessage httpResponse = await client.PutAsync($"payment?id={orderProviderId}", null);
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            throw new ApplicationException($"Access to Pay in the Provider {prov.Nombre} fail with:{httpResponse.ReasonPhrase} {content}");

        }

        public async Task CancelOrderAsync(string orderProviderId)
        {
            // Get the provider by Name
            ProveedorSetting? prov = cconfig.ProveedoresSettings.FirstOrDefault(x => x.Nombre == ProviderName);
            if (prov == null)
            {
                throw new ApplicationException($"Provider unknow for {orderProviderId} ");
            }

            HttpClient client = factory.CreateClient("Provider");
            client.DefaultRequestHeaders.Add("x-api-key", prov.Key);
            client.BaseAddress = new Uri(prov.Url);

            HttpResponseMessage httpResponse = await client.PutAsync($"cancellation?id={orderProviderId}", null);
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                return;
            }

            throw new ApplicationException($"Access to Cancel Pago in the Provider {prov.Nombre} fail with:{httpResponse.ReasonPhrase} {content}");

        }

        private HttpClient HttpCreateClient(HttpContent payload)
        {
            // Configure the Client for the service
            var client = factory.CreateClient("Provider");
            string url = string.Empty;
            foreach (var item in cconfig.ProveedoresSettings)
            {
                if (item.Nombre.ToUpperInvariant() == ProviderName)
                {
                    url = item.Url;
                    payload.Headers.Add("x-api-key", item.Key);
                    break;
                }
                if (url == string.Empty) { throw new ApplicationException($"Provider {ProviderName} not register"); }
                client.BaseAddress = new Uri(url);
            }

            return client;
        }

    }

}
