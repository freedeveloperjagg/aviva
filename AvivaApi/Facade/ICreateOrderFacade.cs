using AvivaLibrary.Models;

namespace AvivaApi.Facade
{
    public interface ICreateOrderFacade
    {
        /// <summary>
        /// et the Status of Cancelation in the order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task CancelOrder(int id);

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <returns></returns>
        Task<List<OrderCreated>> GetAllOrders();
        
        /// <summary>
        /// Get one order by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OrderCreated?> GetOrderById(int id);

        /// <summary>
        /// Insert a Orders in provider
        /// </summary>
        /// <param name="orderCreated"></param>
        /// <returns></returns>
        Task<int> InsertOrderCreatedInTable(OrderCreated orderCreated);
        
        /// <summary>
        /// Mark the order as PAID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task PaidOrder(int id);
    }
}