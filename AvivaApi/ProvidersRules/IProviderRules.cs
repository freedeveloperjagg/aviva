namespace AvivaApi.ProvidersRules
{
    public interface IProviderRules
    {
        public string ProviderName { get; }

        /// <summary>
        /// Return if the method is suitable for the form of payment
        /// andthe fee amount for this provider.
        /// </summary>
        /// <param name="method">The payment Method. This can be different for provider</param>
        /// <param name="amount">The amount of money to pay.</param>
        /// <returns>
        /// true if can process the method
        /// decimal the amount of the fees
        /// if false the returned fee is always 0
        /// </returns>
        (bool, decimal) CalculateFees(string method, decimal amount);
    }
}
