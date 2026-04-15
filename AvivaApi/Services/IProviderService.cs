using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;

namespace AvivaApi.Services
{
    public interface IProviderService
    {
        Task<OrderResponse?> MakePaymentAsync(EntidadDePago provider, OrderPago order);
    }
}
