using AvivaApi.ProvidersRules;
namespace TestAvivaApi.ProviderSelection
{
    public class ProviderCazaPagosRulesTests(ITestOutputHelper xoutput)
    {
        private readonly ProviderCazaPagosRules rules = new();
        private readonly ITestOutputHelper output = xoutput;

        [Fact]
        public void CalculateFees_AmountZero_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            string method = "credit";
            decimal amount = 0;

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => rules.CalculateFees(method, amount));
            Assert.Contains("amount", ex.Message);
        }

        [Theory]
        // Credit method
        [InlineData("credit", 1500, true, 30.00)]   // 2% of 1500
        [InlineData("credit", 5000, true, 75.00)]   // 1.5% of 5000
        [InlineData("credit", 6000, true, 30.00)]   // 0.5% of 6000

        // Transfer method
        [InlineData("transfer", 500, true, 5.00)]   // fixed 5
        [InlineData("transfer", 1000, true, 25.00)] // 2.5% of 1000
        [InlineData("transfer", 2000, true, 40.00)] // 2% of 2000

        // Unsupported method
        [InlineData("cash", 1000, false, 0.00)]
        public void CalculateFees_ReturnsExpectedValues(string method, decimal amount, bool expectedSelected, decimal expectedFee)
        {
            // Act
            var (isSelected, fee) = rules.CalculateFees(method, amount);

            // output
            output.WriteLine($"{isSelected}, {fee}");
            // Assert
            Assert.Equal(expectedSelected, isSelected);
            Assert.Equal(expectedFee, fee);
        }
    }


}
