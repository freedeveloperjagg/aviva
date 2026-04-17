namespace AvivaApi.ProvidersRules
{
    public  class ProviderPagaFacilRules : IProviderRules
    {
        public string ProviderName { get; } = "PAGAFACIL";

        /// <summary>
        /// We need to homologate the methods name before proceed
        /// Standard Methods: cash,credit,transfer
        /// PagaFacil: cash,credit
        /// </summary>
        /// <param name="method"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public (bool, decimal) CalculateFees(string method, decimal amount)
        {
            // Check ifthe amount is not 0
            ArgumentOutOfRangeException.ThrowIfZero(amount, nameof(amount));

            // Check Rules....................
            switch (method.ToUpperInvariant())
            {
                case "CASH":
                    {
                        return (true, 15); // Cash rules: 15 Pesos por Transaccion
                    }
                case "CREDIT":
                    {
                        // Credit Rules: 1% del monto de Transaccion
                        return (true, amount / 100);
                    }
            }
            return (false, 0);
        }
    }
}
