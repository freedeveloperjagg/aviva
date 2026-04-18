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

    readonly ITestOutputHelper output;
    readonly CConfig cconfig;
    public CazaPagosProxyTests(ITestOutputHelper xoutput)
    {
        IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();
        cconfig = new(config);
        this.output = xoutput;
    }

    // Caza pagos response
    string httpContentOkResponse = """
            {
                "orderId": "Y6SbpALt1t",
                "amount": 2615,
                "status": "Pending",
                "method": "METHODPAY",
                "fees": [
                    {
                        "name": "Sales Commision",
                        "amount": 15
                    }
                ],
                "taxes": [
                   {
                     "tax": "Iva",
                     "amount": 10
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
                "createdBy": "apikey-xxxxx"
            }
            """;
    [Theory]
    [InlineData("CASH", null, true, "Payment Method: CASH ")]
    [InlineData("CREDIT", "CreditCard", false, "")]
    [InlineData("TRANSFER", "Transfer", false, "")]
    public async Task CreateOrderAsync_ValidatesPaymentMethod(
     string methodaviva,
     string? methodcazaPagos,
     bool expectException,
     string expectedExceptionMessage)
    {
        // 1. Arrange: Mock the HttpMessageHandler
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        // Change the response based on the method to simulate different scenarios
        httpContentOkResponse = httpContentOkResponse.Replace("METHODPAY", methodcazaPagos);

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
                Content = new StringContent(httpContentOkResponse)
            });

        // 2. Arrange: Setup IHttpClientFactory
        var httpClient = new HttpClient(handlerMock.Object);
        var factoryMock = new Mock<IHttpClientFactory>();
        factoryMock.Setup(f => f.CreateClient("Provider")).Returns(httpClient);

        var proxy = new CazaPagosProxy(factoryMock.Object, cconfig);

        var orderPago = new OrderPago
        {
            Method = methodaviva,
            Products =
            [
                new() { Name = "Toro Sentado", UnitPrice = 250 },
                new() { Name = "Toro Parado",  UnitPrice = 2360 }
            ]
        };

        // 3. Act & Assert
        if (expectException)
        {
            var ex = await Assert.ThrowsAsync<ArgumentException>(
                async () => await proxy.CreateOrderAsync(orderPago));

            Assert.Contains(expectedExceptionMessage, ex.Message);
        }
        else
        {
            // Should complete without throwing and return 200 OK
            var result = await proxy.CreateOrderAsync(orderPago);
            Assert.NotNull(result);
            Assert.IsType<OrderCreated>(result);
            output.WriteLine($"Received OrderCreated with Method: {result.Method}");
            Assert.Equal(methodaviva, result.Method);

        }
    }

}













