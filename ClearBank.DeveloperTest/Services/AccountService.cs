using ClearBank.DeveloperTest.Abstractions;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDataStoreFactory _dataStoreFactory;
        private readonly IConfigurationService _configurationService;

        public AccountService(IDataStoreFactory dataStoreFactory, IConfigurationService configurationService)
        {
            _dataStoreFactory = dataStoreFactory;
            _configurationService = configurationService;
        }

        public Account GetAccount(string accountNumber)
        {
            var dataStore = _dataStoreFactory.GetDataStore(_configurationService.DataStoreType);
            return dataStore.GetAccount(accountNumber);
        }

        public void UpdateAccount(Account account)
        {
            var dataStore = _dataStoreFactory.GetDataStore(_configurationService.DataStoreType);
            dataStore.UpdateAccount(account);
        }
    }
}
