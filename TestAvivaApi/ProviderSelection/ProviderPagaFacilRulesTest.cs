using AvivaApi.ProvidersRules;

namespace TestAvivaApi.ProviderSelection
{
    public class ProviderPagaFacilRulesTest(ITestOutputHelper xoutput)
    {
        private readonly ITestOutputHelper output = xoutput;
        private readonly ProviderPagaFacilRules rules = new ();

        [Fact]
        public void CalculateFees_AmountZero_ThrowsException()
        {
            string method = "cash";
            decimal amount = 0;

            output.WriteLine($"Testing method '{method}' with amount {amount}");

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => rules.CalculateFees(method, amount));

            output.WriteLine($"Exception message: {ex.Message}");
            Assert.Contains("amount", ex.Message);
        }

        [Theory]
        [InlineData("cash", 100, true, 15)]
        [InlineData("credit", 1000, true, 10)] // 1% of 1000
        [InlineData("credit", 2500, true, 25)] // 1% of 2500
        [InlineData("transfer", 500, false, 0)] // Unsupported method
        public void CalculateFees_ReturnsExpectedResults(string method, decimal amount, bool expectedSelected, decimal expectedFee)
        {
            output.WriteLine($"Testing method '{method}' with amount {amount}");

            var (isSelected, fee) = rules.CalculateFees(method, amount);

            output.WriteLine($"Result: isSelected={isSelected}, fee={fee}");

            Assert.Equal(expectedSelected, isSelected);
            Assert.Equal(expectedFee, fee);
        }
    }
}

