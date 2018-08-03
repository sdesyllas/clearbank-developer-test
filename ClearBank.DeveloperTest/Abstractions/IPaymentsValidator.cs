using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Abstractions
{
    public interface IPaymentsValidator
    {
        MakePaymentResult Validate(PaymentScheme paymentScheme, Account account, decimal amount = 0);
    }
}
