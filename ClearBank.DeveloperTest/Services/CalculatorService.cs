using ClearBank.DeveloperTest.Abstractions;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class CalculatorService : ICalculatorService
    {
        public void DeductAmountFromAccount(Account account, decimal amount)
        {
            account.Balance -= amount;
        }
    }
}
