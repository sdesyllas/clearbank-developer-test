using ClearBank.DeveloperTest.Types;
using ClearBank.DeveloperTest.Abstractions;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountService _accountService;
        private readonly IPaymentsValidator _paymentsValidator;
        private readonly ICalculatorService _calculatorService;

        public PaymentService(IPaymentsValidator paymentsValidator, IAccountService accountService, ICalculatorService calculatorService)
        {
            _paymentsValidator = paymentsValidator;
            _accountService = accountService;
            _calculatorService = calculatorService;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _accountService.GetAccount(request.DebtorAccountNumber);
            var result = _paymentsValidator.Validate(request.PaymentScheme, account, request.Amount);
            
            if (!result.Success) return result;
            _calculatorService.DeductAmountFromAccount(account, request.Amount);
            _accountService.UpdateAccount(account);

            return result;
        }
    }
}
