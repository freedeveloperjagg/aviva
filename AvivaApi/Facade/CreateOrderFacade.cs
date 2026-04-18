using AvivaApi.Data;
using AvivaLibrary.Models;

namespace AvivaApi.Facade
{
    public class CreateOrderFacade(OrderRepository xrep) : ICreateOrderFacade
    {
        readonly OrderRepository rep = xrep;

        /// <summary>
        /// Put the facade Status in Cancelled
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task CancelOrder(int id)
        {
            await rep.CancelAsync(id);
        }

        /// <summary>
        /// Get All Orders
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<OrderCreated>> GetAllOrders()
        {
            List<OrderCreated> orders = await rep.GetAllAsync();
            if (orders == null) return [];
            return orders;
        }

        public async Task<OrderCreated?> GetOrderById(int id)
        {
            OrderCreated? order = await rep.GetByIdAsync(id);
            return order;
        }

        /// <summary>
        /// insert OrderCreated in the OrderCreated Table
        /// </summary>
        /// <param name="orderCreated"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<int> InsertOrderCreatedInTable(OrderCreated orderCreated)
        {
            int id = await rep.InsertAsync(orderCreated);
            if (id <= 0)
            {
                throw new ApplicationException($"Fail inserting in DB OrderCreated {orderCreated.ProviderOrderId}: {orderCreated.ProviderName}");
            }

            return id;
        }

        public async Task PaidOrder(int id)
        {
            await rep.PaidAsync(id);
        }
    }
}
