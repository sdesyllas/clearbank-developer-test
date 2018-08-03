using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Abstractions
{
    public interface IAccountService
    {
        Account GetAccount(string accountNumber);
        void UpdateAccount(Account account);
    }
}