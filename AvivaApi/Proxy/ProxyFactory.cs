namespace AvivaApi.Proxy
{   
    /// <summary>
    /// Select the proxy according to the existsnt provider
    /// </summary>
    public class ProxyFactory : IProxyFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ProxyFactory(IServiceProvider xserviceProvider)
        {
            serviceProvider = xserviceProvider;
        }

        /// <summary>
        /// The magic happen here using.
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IProxy GetProxy(string providerName)
        {
            return providerName.ToUpper() switch
            {
                "PAGAFACIL" => serviceProvider.GetRequiredService<PagaFacilProxy>(),
                "CAZAPAGOS" => serviceProvider.GetRequiredService<CazaPagosProxy>(),
                _ => throw new ArgumentException($"Unknown provider: {providerName}")
            };
        }
    }

}
