using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Abstractions
{
    public interface IDataStore
    {
        /// <summary>
        /// Get Account from data store
        /// </summary>
        /// <param name="accountNumber">account number</param>
        /// <returns>account</returns>
        Account GetAccount(string accountNumber);

        /// <summary>
        /// Update account in the data store
        /// </summary>
        /// <param name="account">account to update</param>
        void UpdateAccount(Account account);
    }
}
