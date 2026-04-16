namespace AvivaApi.ProvidersRules
{
    public  class ProviderPagaFacilRules : IProviderRules
    {
        public string ProviderName { get; } = "PagaFacil";

        public (bool, decimal) CalculateFees(string method, decimal amount)
        {
            // Check ifthe amount is not 0
            if (amount == 0) { throw new ArgumentException("The amount of a transaction can not be 0"); }

            // Check Rules....................
            switch (method.ToLowerInvariant())
            {
                case "cash":
                    {
                        return (true, 15); // Cash rules: 15 Pesos por Transaccion
                    }
                case "credit":
                    {
                        // Credit Rules: 1% del monto de Transaccion
                        return (true, amount / 100);
                    }
            }
            return (false, 0);
        }
    }
}
