namespace AvivaApi.Proxy
{
    public interface IProxyFactory
    {
        /// <summary>
        /// Get the proxy to be used
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        IProxy GetProxy(string providerName);
    }
}
