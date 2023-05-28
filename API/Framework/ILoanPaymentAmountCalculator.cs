using JestersCreditUnion.Framework.Enumerations;

namespace JestersCreditUnion.Framework
{
    public interface ILoanPaymentAmountCalculator
    {
        decimal Calculate(decimal totalPrincipal,
            decimal annualInterestRate,
            short term,
            LoanPaymentFrequency paymentFrequency);
    }
}
