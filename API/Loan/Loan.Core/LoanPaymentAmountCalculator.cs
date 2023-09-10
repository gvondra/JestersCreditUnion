using JestersCreditUnion.Loan.Framework;
using JestersCreditUnion.Loan.Framework.Enumerations;
using System;

namespace JestersCreditUnion.Loan.Core
{
    public class LoanPaymentAmountCalculator : ILoanPaymentAmountCalculator
    {
        public decimal Calculate(decimal totalPrincipal,
            decimal annualInterestRate,
            short term,
            LoanPaymentFrequency paymentFrequency)
        {
            double rate = (double)annualInterestRate / (double)paymentFrequency;
            return (decimal)Math.Round(
                (double)totalPrincipal * ((rate * Math.Pow(1.0 + rate, term)) / (Math.Pow(1.0 + rate, term) - 1.0))
                , 5,
                MidpointRounding.ToEven);
        }
    }
}
