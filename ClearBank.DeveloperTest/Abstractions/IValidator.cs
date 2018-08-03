using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Abstractions
{
    public interface IValidator
    {
        MakePaymentResult Validate(Account account, decimal requestAmount = 0);
    }
}
