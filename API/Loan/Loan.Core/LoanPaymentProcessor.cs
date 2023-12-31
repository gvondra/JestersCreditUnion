using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanPaymentProcessor : ILoanPaymentProcessor
    {
        private readonly ILoanSaver _loanSaver;
        private readonly IPaymentSaver _paymentSaver;

        public LoanPaymentProcessor(ILoanSaver loanSaver, IPaymentSaver paymentSaver)
        {
            _loanSaver = loanSaver;
            _paymentSaver = paymentSaver;
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
            await UpdatePayments(settings, paymentTerm.Payments.Where(p => p.Status == PaymentStatus.Unprocessed));
            await _loanSaver.Update(settings, loan);
        }

        private async Task UpdatePayments(ISettings settings, IEnumerable<IPayment> payments)
        {
            foreach (IPayment payment in payments)
            {
                payment.Status = PaymentStatus.Processed;
            }
            await _paymentSaver.Update(settings, payments);
        }

        private static void ProcessTerm(ISettings settings, PaymentTerm paymentTerm)
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
            if (remainingPrincipal <= 0.0M)
                paymentTerm.Loan.Status = LoanStatus.Closed;
            if (paymentTerm.EndDate < DateTime.Today && remainingPrincipal > 0.0M)
            {
                ProcessTerm(settings, paymentTerm.NextTerm(remainingPrincipal));
            }
            else
            {
                paymentTerm.Loan.Balance = remainingPrincipal;
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
