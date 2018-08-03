using ClearBank.DeveloperTest.Abstractions;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceTests
    {
        private readonly Mock<IConfigurationService> _mockConfigurationService;
        private readonly Mock<IDataStore> _mockDataStore;
        private readonly Mock<IDataStoreFactory> _mockDataStoreFactory;
        private readonly Mock<ICalculatorService> _calculatorService;
        private readonly PaymentsValidator _paymentsValidator;

        public PaymentServiceTests()
        {
            _mockConfigurationService = new Mock<IConfigurationService>();
            _mockDataStore = new Mock<IDataStore>();
            _mockDataStoreFactory = new Mock<IDataStoreFactory>();

            _paymentsValidator = new PaymentsValidator();
            _calculatorService = new Mock<ICalculatorService>();
        }
        
        [Fact]
        public void MakePayment_BacsPaymentScheme_And_AccountDoesntExist_Return_False()
        {
            // Arrange
            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => null);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()));

            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);
            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Bacs)
                .Build();

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(1));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            result.Success.Should().BeFalse();
        }

        [Fact]
        public void MakePayment_BacsPaymentScheme_And_AccountExists_AndBacsNotAllowed_Return_False()
        {
            // Arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps)
                .With(x => x.Balance = 100)
                .Build();

            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()));

            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);
            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Bacs)
                .Build();

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(1));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(100);
        }

        [Fact]
        public void MakePayment_BacsPaymentScheme_And_AccountExists_AndBacsIsAllowed_Return_True_And_Update_Balance()
        {
            // Arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs)
                .With(x => x.Balance = 100)
                .Build();

            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Bacs)
                .With(x => x.Amount = 10)
                .Build();
            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()))
                .Callback(() => account.Balance -= makePaymentRequest.Amount);

            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);
            
            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(2));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
            result.Success.Should().BeTrue();
            account.Balance.Should().Be(90);
        }

        [Fact]
        public void MakePayment_ChapsPaymentScheme_And_AccountDoesntExist_Return_False()
        {
            // Arrange
            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => null);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()));

            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);
            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Chaps)
                .Build();

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(1));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            result.Success.Should().BeFalse();
        }

        [Fact]
        public void MakePayment_ChapsPaymentScheme_And_AccountExists_AndChapsNotAllowed_Return_False()
        {
            // Arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs)
                .With(x => x.Balance = 100)
                .Build();

            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()));

            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);
            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Chaps)
                .Build();

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(1));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(100);
        }

        [Fact]
        public void MakePayment_ChapsPaymentScheme_And_AccountExists_AndChapsAllowed_And_Account_Is_Not_Live_Return_False()
        {
            // Arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps)
                .With(x => x.Balance = 100)
                .With(x => x.Status = AccountStatus.Disabled)
                .Build();

            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()));

            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);
            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Chaps)
                .Build();

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(1));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(100);
        }

        [Fact]
        public void MakePayment_ChapsPaymentScheme_And_AccountExists_AndChapsIsAllowed_And_Account_Is_Live_Return_True_And_Update_Balance()
        {
            // Arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps)
                .With(x => x.Balance = 100)
                .With(x => x.Status = AccountStatus.Live)
                .Build();

            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.Chaps)
                .With(x => x.Amount = 10)
                .Build();
            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()))
                .Callback(() => account.Balance -= makePaymentRequest.Amount);

            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);
           
            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(2));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
            result.Success.Should().BeTrue();
            account.Balance.Should().Be(90);
        }

        [Fact]
        public void MakePayment_FasterPaymentsScheme_And_AccountDoesntExist_Return_False()
        {
            // Arrange
            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => null);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()));

            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);
            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.FasterPayments)
                .Build();

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(1));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            result.Success.Should().BeFalse();
        }

        [Fact]
        public void MakePayment_FasterPaymentsScheme_And_AccountExists_AndFasterPaymentsNotAllowed_Return_False()
        {
            // Arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs)
                .With(x => x.Balance = 100)
                .Build();

            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()));

            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);
            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.FasterPayments)
                .Build();

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(1));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(100);
        }

        [Fact]
        public void MakePayment_FasterPaymentsScheme_And_AccountExists_AndFasterPaymentsIsAllowed_And_RequestAmount_Is_More_Than_Balance_Return_False()
        {
            // Arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments)
                .With(x => x.Balance = 100)
                .Build();

            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()));

            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.FasterPayments)
                .With(x => x.Amount = 500)
                .Build();
            
            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);


            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(1));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Never);
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(100);
        }

        [Fact]
        public void MakePayment_FasterPaymentsScheme_And_AccountExists_AndFasterPaymentsIsAllowed_And_RequestAmount_Is_Less_Than_Balance_Return_True_And_Update_Balance()
        {
            // Arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments)
                .With(x => x.Balance = 100)
                .Build();

            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.FasterPayments)
                .With(x => x.Amount = 10)
                .Build();
            _calculatorService.Setup(x => x.DeductAmountFromAccount(account, It.IsAny<decimal>()))
                .Callback(() => account.Balance -= makePaymentRequest.Amount);

            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);


            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(2));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
            result.Success.Should().BeTrue();
            account.Balance.Should().Be(90);
        }

        [Fact]
        public void MakePayment_FasterPaymentsScheme_And_AccountExists_AndFasterPaymentsIsNotAllowedAllowed_And_RequestAmount_Is_Less_Than_Balance_Return_False()
        {
            // Arrange
            var account = Builder<Account>.CreateNew()
                .With(x => x.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs)
                .With(x => x.Balance = 100)
                .Build();

            _mockDataStore.Setup(x => x.GetAccount(It.IsAny<string>())).Returns(() => account);
            _mockDataStoreFactory.Setup(x => x.GetDataStore(It.IsAny<string>())).Returns(_mockDataStore.Object);
            _calculatorService.Setup(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()));

            var paymentService = new PaymentService(_paymentsValidator,
                new AccountService(_mockDataStoreFactory.Object, _mockConfigurationService.Object), _calculatorService.Object);
            MakePaymentRequest makePaymentRequest = Builder<MakePaymentRequest>.CreateNew()
                .With(x => x.PaymentScheme = PaymentScheme.FasterPayments)
                .With(x => x.Amount = 10)
                .Build();

            // Act
            var result = paymentService.MakePayment(makePaymentRequest);

            // Assert
            _mockConfigurationService.Verify(x => x.DataStoreType, Times.Exactly(1));
            _mockDataStore.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Once);
            _mockDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
            _calculatorService.Verify(x => x.DeductAmountFromAccount(It.IsAny<Account>(), It.IsAny<decimal>()),
                Times.Never);
            result.Success.Should().BeFalse();
            account.Balance.Should().Be(100);
        }
    }
}