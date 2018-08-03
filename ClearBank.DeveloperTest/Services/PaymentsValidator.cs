using System.Collections.Generic;
using ClearBank.DeveloperTest.Abstractions;
using ClearBank.DeveloperTest.Common.PaymentSchemeValidators;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentsValidator : IPaymentsValidator
    {
        public Dictionary<PaymentScheme, IValidator> Validators { get; set; }

        public PaymentsValidator()
        {
            Validators = new Dictionary<PaymentScheme, IValidator>
            {
                {PaymentScheme.Bacs, new BacsSchemeValidator()},
                {PaymentScheme.FasterPayments, new FasterPaymentsSchemeValidator()},
                {PaymentScheme.Chaps, new ChapsPaymentsSchemeValidator()}
            };
        }

        public MakePaymentResult Validate(PaymentScheme paymentScheme, Account account, decimal amount = 0)
        {
            return Validators[paymentScheme].Validate(account, amount);
        }
    }
}
