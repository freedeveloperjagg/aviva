namespace AvivaApi.ProvidersRules
{
    /// <summary>
    /// This class has a method to select the provider with the 
    /// small fee in the order.
    /// The update of the different provider is automatic done when you declare in program
    /// the class with the special rules in program and create the specific class.
    /// </summary>
    public class ProviderSelector : IProviderSelector
    {
        private readonly List<IProviderRules> providers;
        public ProviderSelector(List<IProviderRules> xproviders)
        {
            providers = xproviders;
        }

        /// <summary>
        /// Use this to calculate your fees
        /// </summary>
        /// <param name="method"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public string GetBetterProvider(string method, decimal amount)
        {
            decimal fee = decimal.MaxValue;
            string provider = string.Empty;

            foreach (var rules in providers)
            {
                (bool isSelected, decimal feePage) = rules.CalculateFees(method, amount);
                if (isSelected && feePage < fee)
                {
                    fee = feePage;
                    provider = rules.ProviderName; // Add ProviderName to IProviderRules
                }
            }

            return provider;
        }
    }
}
