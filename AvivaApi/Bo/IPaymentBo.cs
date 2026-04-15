using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;

namespace AvivaApi.Bo
{
    public interface IPaymentBo
    {
        Task<OrderResponse> CreatePaymentAsync(OrderPago order);
    }
}