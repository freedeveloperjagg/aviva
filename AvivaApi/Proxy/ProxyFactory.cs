namespace AvivaApi.Proxy
{   
    public class ProxyFactory : IProxyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ProxyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// The magic happen here using polymorphismus.
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IProxy GetProxy(string providerName)
        {
            return providerName.ToLower() switch
            {
                "pagafacil" => _serviceProvider.GetRequiredService<PagaFacilProxy>(),
                "cazapagos" => _serviceProvider.GetRequiredService<CazaPagosProxy>(),
                _ => throw new ArgumentException($"Unknown provider: {providerName}")
            };
        }
    }

}
