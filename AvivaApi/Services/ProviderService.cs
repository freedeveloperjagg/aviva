using AvivaApi.Facade;
using AvivaApi.Proxy;
using AvivaLibrary.Models;

namespace AvivaApi.Services
{
    public class ProviderService(
        IProxyFactory xproxy,
        ICreateOrderFacade xfacade,
        CConfig xconfig) : IProviderService
    {
        readonly IProxyFactory factoryProxy = xproxy;
        readonly CConfig cconfig = xconfig;
        readonly ICreateOrderFacade ofacade = xfacade;

        /// <summary>
        /// Get a Order Created usingthe internal ID
        /// </summary>
        /// <param name="id">Internal Order Id</param>
        /// <returns></returns>
        public async Task<OrderCreated?> GetOrderByIdAsync(int id)
        {
            // Get the provider name and Id from DB
            OrderCreated? order = await ofacade.GetOrderById(id);
            return order;
        }

        public async Task<List<OrderCreated>> GetOrdersAsync()
        {
            // Get Orders from the DB
            List<OrderCreated> ords = await ofacade.GetAllOrders();
            return ords;
        }

        public async Task<OrderCreated?> CreateOrderAsync(string providerName, OrderPago orderPago)
        {
            // Get the proxy
            var proxy = factoryProxy.GetProxy(providerName);

            // Create Order...
            OrderCreated response = await proxy.CreateOrderAsync(orderPago);

            // Store Order in DB
            response.Id = await ofacade.InsertOrderCreatedInTable(response);

            return response;
        }

        /// <summary>
        /// Cancel the order based in the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task CancelOrderAsync(int id)
        {
            // Get the Order in DB to Get the Provider name and Id provider
            OrderCreated? dbresp = await ofacade.GetOrderById(id);
            if (dbresp == null) throw new ApplicationException($"Id {id} not found on the Orders");

            // Get the proxy to be used
            var proxy = factoryProxy.GetProxy(dbresp.ProviderName);
            await proxy.CancelOrderAsync(dbresp.ProviderOrderId);

            //Update the Orders in the DB
            await ofacade.CancelOrder(id);
        }

        public async Task PayOrderAsync(int id)
        {
            // Get the Order in DB to Get the Provider name and Id provider
            OrderCreated? dbresp = await ofacade.GetOrderById(id);
            if (dbresp == null) throw new ApplicationException($"Id {id} not found on the Orders");

            // Get the proxy to be used
            var proxy = factoryProxy.GetProxy(dbresp.ProviderName);
            await proxy.PayOrderAsync(dbresp.ProviderOrderId);

            //Update the Orders in the DB
            await ofacade.PaidOrder(id);
        }
    }
}
