using System.Collections.Generic;
using ClearBank.DeveloperTest.Abstractions;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentsValidatorTests
    {
        private readonly PaymentsValidator _validationService;
        private readonly Mock<IValidator> _bacsValidatorMock;
        private readonly Mock<IValidator> _fasterPaymentsValidatorMock;
        private readonly Mock<IValidator> _chapsValidatorMock;

        public PaymentsValidatorTests()
        {
            _bacsValidatorMock = new Mock<IValidator>();
            _fasterPaymentsValidatorMock = new Mock<IValidator>();
            _chapsValidatorMock = new Mock<IValidator>();

            _validationService = new PaymentsValidator
            {
                Validators = new Dictionary<PaymentScheme, IValidator>
                {
                    {PaymentScheme.Bacs, _bacsValidatorMock.Object},
                    {PaymentScheme.FasterPayments, _fasterPaymentsValidatorMock.Object},
                    {PaymentScheme.Chaps, _chapsValidatorMock.Object}
                }
            };
        }

        [Fact]
        public void Validate_FasterPayments_UsesFasterPaymentsValidator()
        {
            //Arrange
            var paymentScheme = PaymentScheme.FasterPayments;

            //Act
            _validationService.Validate(paymentScheme, new Account());

            //Assert
            _fasterPaymentsValidatorMock.Verify(x => x.Validate(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public void Validate_Chaps_UsesChapsValidator()
        {
            //Arrange
            var paymentScheme = PaymentScheme.Chaps;

            //Act
            _validationService.Validate(paymentScheme, new Account());

            //Assert
            _chapsValidatorMock.Verify(x => x.Validate(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public void Validate_Bacs_UsesBacsValidator()
        {
            //Arrange
            var paymentScheme = PaymentScheme.Bacs;

            //Act
            _validationService.Validate(paymentScheme, new Account());

            //Assert
            _bacsValidatorMock.Verify(x => x.Validate(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
        }
    }
}
