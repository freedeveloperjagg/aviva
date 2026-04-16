using AvivaLibrary.Models;

namespace AvivaApi.Data
{
    public interface IOrderRepository
    {
        Task<int> InsertAsync(OrderCreated order);
        Task UpdateAsync(int id, OrderCreated updatedOrder);
        Task<OrderCreated?> GetByIdAsync(int id);
        Task<List<OrderCreated>> GetAllAsync();
        Task<IEnumerable<OrderCreated>> GetByProviderAsync(string providerName);
        Task CancelAsync(int id);
        Task PaidAsync(int id);
        Task UpdateStatusAsync(int id, string newStatus);
    }

}
