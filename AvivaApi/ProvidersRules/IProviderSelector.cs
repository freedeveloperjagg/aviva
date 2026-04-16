namespace AvivaApi.ProvidersRules
{
    public interface IProviderSelector
    {
        string GetBetterProvider(string method, decimal amount);
    }
}