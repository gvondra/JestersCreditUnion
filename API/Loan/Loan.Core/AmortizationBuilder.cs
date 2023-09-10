using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class AmortizationBuilder : IAmortizationBuilder
    {
        public async Task<IEnumerable<IAmortizationItem>> Build(ISettings settings, ILoan loan)
        {
            short term = 1;
            decimal balance = loan.Agreement.OriginalAmount;
            IEnumerable<IGrouping<short, IPaymentTransaction>> paymentTransactions = (await loan.GetPayments(settings))
                .SelectMany(p => p.Transactions)
                .GroupBy(t => t.TermNumber);
            List<IAmortizationItem> items = new List<IAmortizationItem>
            {
                new AmortizationItem
                {
                    Term = 0,
                    Description = "Original Disbursement",
                    Amount = loan.Agreement.OriginalAmount,
                    Balance = balance
                }
            };
            while (balance > 0.0M && term < 10000)
            {
                decimal interestDue = Math.Round(balance * Rate(loan), 2, MidpointRounding.ToEven);
                bool paymentReceived = false;
                AddPaymentTransactions(
                    in term,
                    ref balance,
                    ref interestDue,
                    ref paymentReceived,
                    items,
                    paymentTransactions
                    .Where(group => group.Key == term)
                    .SelectMany(group => group));

                if (interestDue > 0.0M)
                {
                    items.Add(new AmortizationItem
                    {
                        Term = term,
                        Description = "Interest Due",
                        Amount = interestDue,
                        Balance = balance
                    });
                }

                if (!paymentReceived)
                {
                    decimal principal = loan.Agreement.PaymentAmount - interestDue;
                    if (principal > 0.0M)
                    {
                        if (principal > balance)
                            principal = balance;
                        balance -= principal;
                        items.Add(new AmortizationItem
                        {
                            Term = term,
                            Description = "Principal Due",
                            Amount = principal,
                            Balance = balance
                        });
                    }
                }

                term += 1;
            }
            return items;
        }

        private static void AddPaymentTransactions(
            in short term,
            ref decimal balance,
            ref decimal interestDue,
            ref bool paymentReceived,
            List<IAmortizationItem> items,
            IEnumerable<IPaymentTransaction> paymentTransactions)
        {
            foreach (IPaymentTransaction paymentTransaction in paymentTransactions)
            {
                if (paymentTransaction.Type == TransactionType.PrincipalPayment)
                {
                    balance -= paymentTransaction.Amount;
                    paymentReceived = true;
                }
                else if (paymentTransaction.Type == TransactionType.InterestPayment)
                {
                    interestDue -= paymentTransaction.Amount;
                    paymentReceived = true;
                }
                items.Add(new AmortizationItem
                {
                    Term = term,
                    Description = ToDescription(paymentTransaction.Type),
                    Amount = paymentTransaction.Amount,
                    Balance = balance
                });
            }
        }

        private static decimal Rate(ILoan loan)
            => loan.Agreement.InterestRate / (decimal)loan.Agreement.PaymentFrequency;

        private static string ToDescription(TransactionType transactionType)
        {
            string description = transactionType.ToString();
            switch (transactionType)
            {
                case TransactionType.PrincipalPayment:
                    description = "Principal Payment";
                    break;
                case TransactionType.InterestPayment:
                    description = "Interest Payment";
                    break;
            }
            return description;
        }
    }
}
