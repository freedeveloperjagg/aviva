namespace AvivaApi.Proxy
{
    public interface IProxyFactory
    {
        IProxy GetProxy(string providerName);
    }
}
