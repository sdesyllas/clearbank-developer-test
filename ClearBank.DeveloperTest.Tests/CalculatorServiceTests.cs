using ClearBank.DeveloperTest.Abstractions;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class CalculatorServiceTests
    {
        private readonly ICalculatorService _calculatorService;

        public CalculatorServiceTests()
        {
            _calculatorService = new CalculatorService();
        }

        [Fact]
        public void DeductAmountFromAccount_WithDeductRequest_Deduct_Money_From_Account()
        {
            // Arrange
            var account = Builder<Account>.CreateNew().With(x => x.Balance = 100).Build();

            // Act
            _calculatorService.DeductAmountFromAccount(account, 10);

            // Assert
            account.Balance.Should().Be(90);
        }
    }
}
