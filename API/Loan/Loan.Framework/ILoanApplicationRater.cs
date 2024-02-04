namespace JestersCreditUnion.Loan.Framework
{
    public interface ILoanApplicationRater
    {
        IRating Rate(ILoanApplication loanApplication);
    }
}
