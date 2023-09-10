namespace JestersCreditUnion.Loan.Framework
{
    public interface IAmortizationItem
    {
        short Term { get; }
        string Description { get; }
        decimal Amount { get; }
        decimal Balance { get; }
    }
}
