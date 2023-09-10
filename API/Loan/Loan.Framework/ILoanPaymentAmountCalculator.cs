using JestersCreditUnion.Loan.Framework.Enumerations;

namespace JestersCreditUnion.Loan.Framework
{
    public interface ILoanPaymentAmountCalculator
    {
        decimal Calculate(decimal totalPrincipal,
            decimal annualInterestRate,
            short term,
            LoanPaymentFrequency paymentFrequency);
    }
}
