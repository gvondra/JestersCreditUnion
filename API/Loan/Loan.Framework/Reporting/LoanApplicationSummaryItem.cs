namespace JestersCreditUnion.Loan.Framework.Reporting
{
    public readonly struct LoanApplicationSummaryItem
    {
        public LoanApplicationSummaryItem(short year, short month, int count, string description)
        {
            Year = year;
            Month = month;
            Count = count;
            Description = description;
        }

        public short Year { get; init; }
        public short Month { get; init; }
        public int Count { get; init; }
        public string Description { get; init; }
    }
}
