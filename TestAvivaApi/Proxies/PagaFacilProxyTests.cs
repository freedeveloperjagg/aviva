using AvivaApi;
using AvivaLibrary.Models;
using global::AvivaApi.Proxy;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using System.Net;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
namespace TestAvivaApi.Proxies;

public class PagaFacilProxyTests
{

    CConfig cconfig;
    public PagaFacilProxyTests()
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
                "orderId": "4f6d499d-9766-46d4-ad2d-54bdde6712ee",
                "amount": 2615,
                "status": "Pending",
                "method": "Cash",
                "fees": [
                    {
                        "name": "Sales Commision",
                        "amount": 15
                    }
                ],
                "products": [
                    {
                        "name": "Toro Sentado",
                        "unitPrice": 250
                    },
                    {
                        "name": "Toro Parado",
                        "unitPrice": 2350
                    }
                ],
                "createdDate": "2026-04-17T03:16:07.9603899+00:00",
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
      
        var proxy = new PagaFacilProxy(factoryMock.Object, cconfig);
        var orderPago = new OrderPago() 
        { 
            Method = "CASH",
            Products = [ new(){ Name="Toro Sentado", UnitPrice = 250 }, new() { Name = "Toro Parado", UnitPrice = 2360 }]
        }; // Fill with required data for factory

        // Act
        var result = await proxy.CreateOrderAsync(orderPago);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderPago.Method, result.Method);
        Assert.True(result.Fees.Count == 1);
        Assert.True(result.Products.Count == 2);
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}











