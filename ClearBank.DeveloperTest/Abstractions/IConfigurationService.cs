namespace ClearBank.DeveloperTest.Abstractions
{
    public interface IConfigurationService
    {
        /// <summary>
        /// Get Data store type
        /// </summary>
        string DataStoreType { get; }
    }
}
