using ClearBank.DeveloperTest.Abstractions;
using ClearBank.DeveloperTest.Data;

namespace ClearBank.DeveloperTest.Factories
{
    public class DataStoreFactory : IDataStoreFactory
    {
        private const string BackUp = "Backup";

        public IDataStore GetDataStore(string dataStoreType)
        {
            return dataStoreType == BackUp ? (IDataStore) new BackupAccountDataStore() : new AccountDataStore();
        }
    }
}
