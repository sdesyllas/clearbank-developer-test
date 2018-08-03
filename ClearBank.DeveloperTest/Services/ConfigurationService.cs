using System.Configuration;
using ClearBank.DeveloperTest.Abstractions;

namespace ClearBank.DeveloperTest.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public string DataStoreType => ConfigurationManager.AppSettings["DataStoreType"];
    }
}
