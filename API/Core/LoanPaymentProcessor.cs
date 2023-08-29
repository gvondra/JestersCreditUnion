using JestersCreditUnion.Framework;
using JestersCreditUnion.Framework.Enumerations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class LoanPaymentProcessor : ILoanPaymentProcessor
    {
        private readonly ITransactionFacatory _transactionFactory;

        public LoanPaymentProcessor(ITransactionFacatory transactionFactory)
        {
            _transactionFactory = transactionFactory;
        }

        public async Task Process(ISettings settings, ILoan loan)
        {
            PaymentTerm paymentTerm = new PaymentTerm
            {
                TermNumber = 1,
                Loan = loan,
                Payments = (await loan.GetPayments(settings)).ToArray(),
                StartDate = loan.InitialDisbursementDate.Value,
                EndDate = loan.FirstPaymentDue.Value,
                Principal = loan.Agreement.OriginalAmount
            };
            ProcessTerm(settings, paymentTerm);
        }

        private void ProcessTerm(ISettings settings, PaymentTerm paymentTerm)
        {
            decimal remainingPrincipal = paymentTerm.Principal;
            decimal interestDue = InterestDue(paymentTerm);
            foreach (IPayment payment in paymentTerm.Payments
                .Where(p => p.Status == PaymentStatus.Unprocessed)
                .OrderBy(p => p.Date))
            {
                decimal paymentAmount = PaymentAmount(payment);
                decimal interestAmount = Math.Min(
                    Math.Round(paymentTerm.Principal * Rate(paymentTerm.Loan), 2, MidpointRounding.ToEven),
                    paymentAmount);
                decimal principalAmount = 0.0M;
                if (payment.Date <= paymentTerm.EndDate)
                {
                    principalAmount = Math.Max(
                        paymentAmount - interestAmount,
                        0.0M);
                }
                interestDue -= interestAmount;
                remainingPrincipal -= principalAmount;
                SetPaymentTransactions(paymentTerm.Loan, payment, paymentTerm.TermNumber, interestAmount, principalAmount);
                if (interestDue == 0.0M && paymentTerm.Loan.NextPaymentDue.Value == paymentTerm.EndDate)
                    paymentTerm.Loan.NextPaymentDue = PaymentDateCalculator.NextPaymentDate(paymentTerm.Loan.NextPaymentDue.Value, paymentTerm.Loan.Agreement.PaymentFrequency);
            }
            if (paymentTerm.EndDate < DateTime.Today)
            {
                ProcessTerm(settings, paymentTerm.NextTerm(remainingPrincipal));
            }
        }

        private static void SetPaymentTransactions(
            ILoan loan,
            IPayment payment,
            short termNumber,
            decimal interestAmount,
            decimal principalAmount)
        {
            IPaymentTransaction paymentTransaction;
            if (interestAmount > 0.0M)
            {
                paymentTransaction = payment.CreatePaymentTransaction(loan, DateTime.Today, TransactionType.InterestPayment, interestAmount);
                paymentTransaction.TermNumber = termNumber;
                payment.Transactions.Add(paymentTransaction);
            }
            if (principalAmount > 0.0M)
            {
                paymentTransaction = payment.CreatePaymentTransaction(loan, DateTime.Today, TransactionType.PrincipalPayment, principalAmount);
                paymentTransaction.TermNumber = termNumber;
                payment.Transactions.Add(paymentTransaction);
            }
        }

        private static decimal Rate(ILoan loan)
            => loan.Agreement.InterestRate / (decimal)loan.Agreement.PaymentFrequency;

        private static decimal PaymentAmount(IPayment payment)
        {
            decimal amount = payment.Amount;
            amount -= payment.Transactions.Sum(t => t.Amount);
            return amount;
        }

        private static decimal InterestDue(PaymentTerm paymentTerm)
        {
            decimal interestDue = Math.Round(paymentTerm.Principal * Rate(paymentTerm.Loan), 2, MidpointRounding.ToEven);
            interestDue -= paymentTerm.Payments
                .SelectMany(p => p.Transactions)
                .Where(t => t.Type == TransactionType.InterestPayment && t.TermNumber == paymentTerm.TermNumber)
                .Sum(t => t.Amount);
            return interestDue;
        }

        private struct PaymentTerm
        {
            public short TermNumber { get; set; }
            public ILoan Loan { get; set; }
            public IPayment[] Payments { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public decimal Principal { get; set; }

            public PaymentTerm NextTerm(decimal principal)
            {
                DateTime start = this.EndDate.AddDays(1);
                return new PaymentTerm
                {
                    TermNumber = (short)(this.TermNumber + 1),
                    Loan = this.Loan,
                    Payments = this.Payments,
                    StartDate = start,
                    EndDate = PaymentDateCalculator.NextPaymentDate(start, this.Loan.Agreement.PaymentFrequency),
                    Principal = principal
                };
            }
        }
    }
}
