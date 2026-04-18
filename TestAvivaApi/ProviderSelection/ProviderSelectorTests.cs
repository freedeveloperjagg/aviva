namespace TestAvivaApi
{
    using AvivaApi.ProvidersRules;
    using System;
    using System.Collections.Generic;
    using Xunit;

    namespace MyApi.Tests
    {
        public class ProviderSelectorTests
        {
            private readonly ProviderSelector selector;

            public ProviderSelectorTests()
            {
                // Here you must initialize with your real provider rules
                // Replace with actual provider rules from your production code
                var providers = new List<IProviderRules>
            {
                new ProviderCazaPagosRules(),
                new ProviderPagaFacilRules()
            };

                selector = new ProviderSelector(providers);
            }

            [Fact]
            public void GetBetterProvider_AmountZero_ThrowsException()
            {
                // Arrange
                string method = "CASH";
                decimal amount = 0;

                // Act & Assert
                Assert.Throws<ArgumentOutOfRangeException>(() => selector.GetBetterProvider(method, amount));
            }

            [Theory]
            [InlineData("CASH", 10000, "PAGAFACIL")]
            [InlineData("CASH", 1000, "PAGAFACIL")]
            [InlineData("CREDIT", 1000, "PAGAFACIL")]
            [InlineData("CREDIT", 1520, "PAGAFACIL")]
            [InlineData("CREDIT", 6000, "CAZAPAGOS")]
            [InlineData("TRANSFER", 10000, "CAZAPAGOS")]
            [InlineData("TRANSFER", 10, "CAZAPAGOS")]
            public void GetBetterProvider_ValidInputs_ReturnsExpectedProvider(string method, decimal amount, string expectedProvider)
            {
                // Act
                var result = selector.GetBetterProvider(method, amount);

                // Assert
                Assert.Equal(expectedProvider, result);
            }
        }
    }



}
