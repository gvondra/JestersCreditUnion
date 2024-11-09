namespace JestersCreditUnion.Loan.Framework.Reporting
{
    public readonly struct LoanPastDue
    {
        public string Number { get; init; }
        public short DaysPastDue { get; init; }
    }
}
