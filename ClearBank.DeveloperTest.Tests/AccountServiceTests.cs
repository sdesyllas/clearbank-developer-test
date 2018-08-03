using ClearBank.DeveloperTest.Abstractions;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class AccountServiceTests
    {
        private readonly Mock<IConfigurationService> _mockConfigurationService;
        private readonly Mock<IDataStoreFactory> _mockDataStoreFactory;
        private readonly Mock<IDataStore> _mockDataStore;
        private IAccountService _accountService;

        public AccountServiceTests()
        {
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockDataStoreFactory = new Mock<IDataStoreFactory>();
            _mockDataStore = new Mock<IDataStore>();
        }

        [Fact]
        public void GetAccount_ExistingAccountNumber_Return_Account()
        {
            // Arrange
            _mockConfigurationService.Setup(x=>x.DataStoreType).Verifiable();
            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(new Account());
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _accountService = new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object);

            // Act
            var account = _accountService.GetAccount("existing account number");

            // Assert
            _mockDataStoreFactory.Verify(x => x.GetDataStore(It.IsAny<string>()), Times.Once);
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Once);
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            account.Should().NotBeNull();
        }

        [Fact]
        public void GetAccount_NonExistingAccountNumber_Return_Null()
        {
            // Arrange
            _mockConfigurationService.Setup(x => x.DataStoreType).Verifiable();
            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => null);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _accountService = new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object);

            // Act
            var account = _accountService.GetAccount("non existing account number");

            // Assert
            _mockDataStoreFactory.Verify(x => x.GetDataStore(It.IsAny<string>()), Times.Once);
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Once);
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            account.Should().BeNull();
        }

        [Fact]
        public void UpdateAccount_Account_UpdateInDataStore()
        {
            // Arrange
            _mockConfigurationService.Setup(x => x.DataStoreType).Verifiable();
            _mockDataStore.Setup(x => x.UpdateAccount(It.IsAny<Account>())).Verifiable();
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _accountService = new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object);

            // Act
            _accountService.UpdateAccount(new Account());

            // Assert
            _mockDataStoreFactory.Verify(x => x.GetDataStore(It.IsAny<string>()), Times.Once);
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
        }
    }
}
