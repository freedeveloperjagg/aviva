using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
using Microsoft.AspNetCore.Http.Json;
using System.Text;
using System.Text.Json;

namespace AvivaApi.Proxy
{
    public class PaymentProxy(
        IHttpClientFactory xfactory,
        CConfig xconfig
        ) :IPaymentProxy
    {
        private readonly IHttpClientFactory factory = xfactory;
        private readonly CConfig cconfig = xconfig;
        private readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true,
        };
        


        public async Task<OrderResponse?> CreateOrderAsync(EntidadDePago provider, OrderPago orderPago)
        {
            // Create the payload
            string json = JsonSerializer.Serialize( orderPago );
            HttpContent payload = new StringContent(json, Encoding.UTF8, "application/json");
            payload.Headers.Add("x-api-key", "apikey-1cnmoisyhkif7s");
                     
            HttpClient client = factory.CreateClient("Provider");                     
            
            // Get the Url for the service
            string url = string.Empty;
            foreach (var item in cconfig.ProveedoresSettings)
            {
                if (item.Nombre == provider.Nombre)
                {
                    url = item.Url;
                    break;
                }
            }

            if (url == string.Empty) { throw new ApplicationException($"Provider {provider.Nombre} not register"); }
           
            client.BaseAddress = new Uri(url);
            var result = await client.PostAsync("Order", payload);
             string content = await result.Content.ReadAsStringAsync();
            if (result.IsSuccessStatusCode)
            { 
               
                OrderResponse? order = JsonSerializer.Deserialize<OrderResponse>(content,options);
                return order;          
            }
            throw new ApplicationException($"Error reading the proxy in {url}, Error: {content}");
        }  

    }
}
