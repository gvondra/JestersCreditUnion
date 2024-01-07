namespace JestersCreditUnion.Loan.Framework.Reporting
{
    public readonly struct LoanApplicationSummaryItem
    {
        public LoanApplicationSummaryItem(int year, int month, int count, string description)
        {
            Year = year;
            Month = month;
            Count = count;
            Description = description;
        }

        public int Year { get; init; }
        public int Month { get; init; }
        public int Count { get; init; }
        public string Description { get; init; }
    }
}
