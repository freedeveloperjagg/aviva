using AvivaApi.Facade;
using AvivaApi.ProvidersRules;
using AvivaApi.Services;
using AvivaLibrary.Models;
using AvivaLibrary.Models.Requests;

namespace AvivaApi.Bo
{
    public class PaymentBo(
        IPaymentCompaniesFacade xfacade,
        IProviderSelector xselector,
        IProviderService providerService
        ) : IPaymentBo
    {
        readonly IPaymentCompaniesFacade facade = xfacade;
        readonly IProviderService service = providerService;
        readonly IProviderSelector selector = xselector;

 
        /// <summary>
        /// We need to made the payment to the entity that cost less, 
        /// so we need to calculate the total cost of the order and then get the 
        /// list of companies that can make the payment and then select 
        /// the one that cost less and then call the service to made the payment
        /// </summary>
        /// <param name="orderPago"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<OrderCreated?> CreatePaymentAsync(OrderPago orderPago)
        {
            // We need to create the payment using the entity that cost less

            // First we need to calculate the total cost of the order
            decimal totalCost = orderPago.Products.Sum(p => p.UnitPrice);

            // Then we need to know who is the better provider for this order
            string providerName = selector.GetBetterProvider(orderPago.Method, totalCost);
                        
            // See if  provider was elected
            if (string.IsNullOrEmpty(providerName))
            {
                throw new Exception("No provider was elected");
            }

           
            // Call the service to made the payment
            OrderCreated? order = await service.CreateOrderAsync(providerName, orderPago);
            return order;
        }

        /// <summary>
        /// Get a individual Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OrderCreated?> GetOrderByIdAsync(int id)
        {
            OrderCreated? response = await service.GetOrderByIdAsync(id)   ;
            return response;
        }

        /// <summary>
        /// Get All Order s in all providers
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrderCreated>> GetOrdersAsync()
        {
            List<OrderCreated> orders = await service.GetOrdersAsync();
            return orders;
        }

        /// <summary>
        /// Pay a order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task PayOrderAsync(int id)
        {
            await service.PayOrderAsync(id);
        }

        /// <summary>
        /// Cancel Order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CancelOrderAsync(int id)
        {
            await service.CancelOrderAsync(id);
        }
    }
}
