namespace AvivaApi.Proxy
{
    /// <summary>
    /// Select the proxy according to the existsnt provider
    /// </summary>
    public class ProxyFactory(IServiceProvider xserviceProvider) : IProxyFactory
    {
        private readonly IServiceProvider serviceProvider = xserviceProvider;

        /// <summary>
        /// The magic happen here using.
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IProxy GetProxy(string providerName)
        {
            providerName = providerName.ToUpperInvariant();
            return providerName switch
            {
                "PAGAFACIL" => serviceProvider.GetRequiredService<PagaFacilProxy>(),
                "CAZAPAGOS" => serviceProvider.GetRequiredService<CazaPagosProxy>(),
                _ => throw new ArgumentException($"Unknown provider: {providerName}")
            };
        }
    }

}
