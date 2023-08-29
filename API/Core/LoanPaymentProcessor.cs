using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class LoanPaymentProcessor : ILoanPaymentProcessor
    {
        public async Task Process(ISettings settings, IPayment payment)
        {
            ILoan loan = await payment.GetLoan(settings);
            PaymentTerm paymentTerm = new PaymentTerm
            {
                Loan = loan,
                Payment = payment,
                StartDate = loan.FirstPaymentDue.Value,
                EndDate = PaymentDateCalculator.NextPaymentDate(loan.FirstPaymentDue.Value, loan.Agreement.PaymentFrequency),
                Principal = loan.Agreement.OriginalAmount
            };
            await ProcessTerm(settings, paymentTerm);
        }

        private async Task ProcessTerm(ISettings settings, PaymentTerm paymentTerm)
        {
            decimal totalPrincipal = paymentTerm.Principal;
            if (paymentTerm.StartDate <= paymentTerm.Payment.Date && paymentTerm.Payment.Date <= paymentTerm.EndDate)
            {
                decimal interestAmount = Math.Min(
                    Math.Round(totalPrincipal * paymentTerm.Loan.Agreement.InterestRate, 2, MidpointRounding.ToEven),
                    paymentTerm.Payment.Amount);
                decimal principalAmount = Math.Min(
                    paymentTerm.Payment.Amount - interestAmount,
                    0.0M);
            }
            if (paymentTerm.HasNextTerm())
            {
                await ProcessTerm(settings, paymentTerm.NextTerm(totalPrincipal));
            }
        }

        //private decimal InterestDue(PaymentTerm paymentTerm)
        //{
        //    decimal interestDue = Math.Round(paymentTerm.Principal * paymentTerm.Loan.Agreement.InterestRate, 2, MidpointRounding.ToEven);
            
        //}

        private struct PaymentTerm
        {
            public ILoan Loan { get; set; }
            public IPayment Payment { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal Principal { get; set; }

            public readonly bool HasNextTerm() => EndDate < Payment.Date && DateTime.Today <= EndDate;

            public PaymentTerm NextTerm(decimal principal)
            {
                DateTime start = this.EndDate.AddDays(1);
                return new PaymentTerm
                {
                    Loan = this.Loan,
                    Payment = this.Payment,
                    StartDate = start,
                    EndDate = PaymentDateCalculator.NextPaymentDate(start, this.Loan.Agreement.PaymentFrequency),
                    Principal = principal
                };
            }
        }
    }
}
