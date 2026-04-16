
namespace AvivaApi.ProvidersRules
{
    public class ProviderCazaPagosRules : IProviderRules
    {
        public string ProviderName { get; } = "CazaPagos";

        public (bool, decimal) CalculateFees(string method, decimal amount)
        {
            // Check ifthe amount is not 0
            if (amount == 0) { throw new ArgumentException("The amount of a transaction can not be 0"); }

            // Check Rules....................
            switch (method.ToLowerInvariant())
            {
                case "creditcard":
                    {
                        if (amount <= 1500) return (true, amount / 50); // 2%
                        if (amount <= 5000) return (true, 1.5m * amount / 100); // 1.5 %
                        return (true, amount / 200); // 0.5 %
                     }
                case "transfer":
                    {
                        if (amount <= 500) return (true, 5m); // 2%
                        if (amount <= 1000) return (true, 2.5m * amount / 100); // 2.5 %
                        return (true, amount / 50); // 2 %
                    }
             }
                return (false, 0);
        }
    }
}
