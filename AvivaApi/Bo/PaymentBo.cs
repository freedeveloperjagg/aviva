using AvivaApi.Facade;
using AvivaApi.Services;
using AvivaLibrary.Models;
using AvivaLibrary.Models.Requests;
using AvivaLibrary.Models.Responses;

namespace AvivaApi.Bo
{
    public class PaymentBo(
        IPaymentCompaniesFacade xfacade,
        IProviderService providerService
        ) : IPaymentBo
    {
        readonly IPaymentCompaniesFacade facade = xfacade;
        readonly IProviderService service = providerService;

 
        /// <summary>
        /// We need to made the payment to the entity that cost less, 
        /// so we need to calculate the total cost of the order and then get the 
        /// list of companies that can make the payment and then select 
        /// the one that cost less and then call the service to made the payment
        /// </summary>
        /// <param name="orderPago"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrderResponse?> CreatePaymentAsync(OrderPago orderPago)
        {
            // We need to create the payment using the entity that cost less

            // First we need to calculate the total cost of the order
            decimal totalCost = orderPago.Products.Sum(p => p.UnitPrice);

            // Get the list of comapanies that can make the payment
            var providers = await facade.GetAllProvidersAsync();

            decimal? value = decimal.MaxValue;
            string providerName = string.Empty;
            foreach (var prov in providers)
            {
                var charge = await facade.CalculateFeeAsync(prov.Nombre, orderPago.Method, totalCost);
                if (charge < value)
                {
                    value = charge;
                    providerName = prov.Nombre;
                }
            }

            // See if  provider was elected
            if (string.IsNullOrEmpty(providerName))
            {
                throw new Exception("No provider was elected");
            }

            // Select the provider and create the payment...
            var provider = providers.Single(x => x.Nombre == providerName);

            // Call the service to made the payment
            OrderResponse? order = await service.CreateOrderAsync(provider, orderPago);
            return order;
        }

        /// <summary>
        /// Get a individual Order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public async Task<OrderResponse?> GetOrderAsync(string id, string provider)
        {
            OrderResponse? response = await service.GetOrderAsync(id, provider)   ;
            return response;
        }

        /// <summary>
        /// Get All Order s in all providers
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrderResponse>> GetOrdersAsync()
        {
            List<OrderResponse> orders = await service.GetOrdersAsync();
            return orders;
        }

        /// <summary>
        /// Pay a order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task PayOrderAsync(ChangeOrderRequest request)
        {
            await service.PayOrderAsync(request);
        }

        /// <summary>
        /// Cancel Order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CancelOrderAsync(ChangeOrderRequest request)
        {
            await service.CancelOrderAsync(request);
        }
    }
}
