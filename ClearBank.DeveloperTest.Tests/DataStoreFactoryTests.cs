using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Factories;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class DataStoreFactoryTests
    {
        [Fact]
        public void GetDataStore_WithBackupDatastoreConfig_Factory_To_Create_BackupDataStore()
        {
            // Arrange
            var factory = new DataStoreFactory();

            // Act
            var dataStore = factory.GetDataStore("Backup");

            // Assert
            dataStore.Should().BeOfType<BackupAccountDataStore>();
        }

        [Fact]
        public void GetDataStore_WithNormalDataStoreConfig_Factory_To_Create_NormalDataStore()
        {
            // Arrange
            var factory = new DataStoreFactory();

            // Act
            var dataStore = factory.GetDataStore("");

            // Assert
            dataStore.Should().BeOfType<AccountDataStore>();
        }
    }
}
