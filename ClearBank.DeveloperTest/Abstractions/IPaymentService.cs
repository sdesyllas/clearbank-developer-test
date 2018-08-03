using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Abstractions
{
    public interface IPaymentService
    {
        /// <summary>
        /// Make payment
        /// </summary>
        /// <param name="request">payment request</param>
        /// <returns>payment result</returns>
        MakePaymentResult MakePayment(MakePaymentRequest request);
    }
}
