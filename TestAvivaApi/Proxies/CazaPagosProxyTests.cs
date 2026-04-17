using AvivaApi;
using AvivaLibrary.Models;
using global::AvivaApi.Proxy;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
namespace TestAvivaApi.Proxies;

public class CazaPagosProxyTests
{

    CConfig cconfig;
    public CazaPagosProxyTests()
    {
        IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        cconfig = new(config);
    }

    [Fact]
    public async Task CreateOrderAsync_ReturnsOrderCreated_OnSuccess()
    {
        // 1. Arrange: Mock the HttpMessageHandler
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        // Response...
        string jsonResponse = """
            {
                "orderId": "lyxdDUiTuO",
                "amount": 293.25,
                "status": "Pending",
                "method": "CreditCard",
                "fees": [
                    {
                        "title": "Sales Commision",
                        "amount": 5
                    }
                ],
                "taxes": [
                    {
                        "tax": "IVA",
                        "amount": 38.25
                    }
                ],
                "products": [
                    {
                        "name": "Toro Sentado",
                        "unitPrice": 250
                    }
                ],
                "createdDate": "2026-04-17T02:37:02.1041954+00:00",
                "createdBy": "apikey-1cnmoisyhkif7s"
            }
            """;
            
        // Setup deterministic response for any PostAsync call
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse) // Mock JSON response
            });

        // 2. Arrange: Setup IHttpClientFactory to return a client using the mock handler
        var httpClient = new HttpClient(handlerMock.Object);
        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(f => f.CreateClient("Provider")).Returns(httpClient);
      
        var proxy = new CazaPagosProxy(factoryMock.Object, cconfig);
        var orderPago = new OrderPago() 
        { 
            Id = 10,
            Method = "CREDIT",
            Products = [ new(){ Name="Toro Sentado", UnitPrice = 250 }]
        }; // Fill with required data for factory

        // Act
        var result = await proxy.CreateOrderAsync(orderPago);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Method, orderPago.Method);
        Assert.True(result.Fees.Count == 2);
        Assert.True(result.Products.Count == 1);
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}











