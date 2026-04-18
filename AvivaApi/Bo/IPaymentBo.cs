using AvivaLibrary.Models;

namespace AvivaApi.Bo
{
    public interface IPaymentBo
    {
        Task<OrderCreated?> CreatePaymentAsync(OrderPago order);
        Task<OrderCreated?> GetOrderByIdAsync(int id);
        Task<List<OrderCreated>> GetOrdersAsync();
        Task PayOrderAsync(int id);
        Task CancelOrderAsync(int id);

    }
}