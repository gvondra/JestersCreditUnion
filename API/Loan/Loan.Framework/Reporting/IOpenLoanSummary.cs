namespace JestersCreditUnion.Loan.Framework.Reporting
{
    public interface IOpenLoanSummary
    {
        string Number { get; }
        decimal Balance { get; }
        DateTime NextPaymentDue { get; }
    }
}
