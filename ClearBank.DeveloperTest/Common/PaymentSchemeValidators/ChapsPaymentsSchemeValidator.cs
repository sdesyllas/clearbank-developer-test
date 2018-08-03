using ClearBank.DeveloperTest.Abstractions;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Common.PaymentSchemeValidators
{
    public class ChapsPaymentsSchemeValidator : IValidator
    {
        public MakePaymentResult Validate(Account account, decimal requestAmount = 0)
        {
            var result = new MakePaymentResult();
            if (account != null && account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) &&
                account.Status == AccountStatus.Live)
            {
                result.Success = true;
            }
            return result;
        }
    }
}
