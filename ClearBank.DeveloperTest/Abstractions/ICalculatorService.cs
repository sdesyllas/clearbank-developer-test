using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Abstractions
{
    public interface ICalculatorService
    {
        void DeductAmountFromAccount(Account account, decimal amount);
    }
}