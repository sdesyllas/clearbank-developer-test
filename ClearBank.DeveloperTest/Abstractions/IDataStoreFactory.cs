namespace ClearBank.DeveloperTest.Abstractions
{
    public interface IDataStoreFactory
    {
        IDataStore GetDataStore(string dataStoreType);
    }
}
