using AvivaApi;
using AvivaApi.Proxy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestAvivaApi.Proxies
{
    public class ProxySelectorTests
    {
        private readonly ITestOutputHelper output;
        private readonly IServiceProvider serviceProvider;
        private readonly ProxyFactory proxyFactory;
        private readonly CConfig cconfig;

        public ProxySelectorTests(ITestOutputHelper xoutput)
        {
            output = xoutput;
            // Creating the configuration
            IConfiguration config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
            cconfig = new(config);


            // Build a real ServiceCollection just like Program.cs
            var services = new ServiceCollection();

            // Register the proxies under test
            services.AddHttpClient("Provider", client =>
            {
                client.BaseAddress = new Uri("https://fake-provider.com");
            });

            // Register dependencies each proxy needs
            services.AddSingleton<CConfig>(cconfig);
            services.AddTransient<PagaFacilProxy>();
            services.AddTransient<CazaPagosProxy>();

            serviceProvider = services.BuildServiceProvider();
            proxyFactory = new ProxyFactory(serviceProvider);

        }


        [Theory]
        [InlineData("PAGAFACIL", false)]
        [InlineData("PagaFacil", false)]
        [InlineData("CAZAPAGOS", false)]
        [InlineData("CazaPagos", false)]
        [InlineData("FAKEPROVIDER", true)]
        public async Task ProxySelectorSelectPagaFacilAsync(string providerName, bool isException)
        {
            if (isException)
            {
                var ex = await Assert.ThrowsAsync<ArgumentException>(
                async () => proxyFactory.GetProxy(providerName));
                Assert.Contains("Unknown provider", ex.Message);
            }
            else
            {
                var proxy = proxyFactory.GetProxy(providerName);
                Assert.Equal(providerName.ToUpperInvariant(), proxy.ProviderName);
                output.WriteLine(proxy.ProviderName);
            }
        }

    }
}
