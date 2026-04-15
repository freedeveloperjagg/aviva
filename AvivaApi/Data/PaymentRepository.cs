namespace AvivaApi.Data
{
    using AvivaLibrary.Models;
    using Microsoft.EntityFrameworkCore;

    public class PaymentRepository : IPaymentRepository
    {
        private readonly AvivaDbContext context;

        public PaymentRepository(AvivaDbContext xcontext)
        {
            context = xcontext;
        }

        public async Task<IEnumerable<EntidadDePago>> GetAllProvidersAsync()
        {
            return await context.EntidadesDePago
                .Include(e => e.Rules)
                    .ThenInclude(r => r.Limites)
                .ToListAsync();
        }

        public async Task<EntidadDePago?> GetProviderByNameAsync(string nombre)
        {
            return await context.EntidadesDePago
                .Include(e => e.Rules)
                    .ThenInclude(r => r.Limites)
                .FirstOrDefaultAsync(e => e.Nombre.ToLower() == nombre.ToLower());
        }

        public async Task<decimal?> CalculateFeeAsync(string providerName, string paymentType, decimal amount)
        {
            var provider = await GetProviderByNameAsync(providerName);
            if (provider == null) return null;

            // Find the rule that matches the payment type and the amount range
            var limit = provider.Rules
                .Where(r => r.TipoPago.Equals(paymentType, StringComparison.OrdinalIgnoreCase))
                .SelectMany(r => r.Limites)
                .FirstOrDefault(l =>
                    (amount >= l.Min) &&
                    (l.Max == 0 || amount <= l.Max));

            if (limit == null) return null;

            // Calculate based on type: 'P' (Percentage) or 'F' (Fixed)
            return limit.Type == 'P'
                ? amount * (limit.Charge / 100)
                : limit.Charge;
        }
    }

}
