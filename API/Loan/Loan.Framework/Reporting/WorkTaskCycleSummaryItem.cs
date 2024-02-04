namespace JestersCreditUnion.Loan.Framework.Reporting
{
    public readonly struct WorkTaskCycleSummaryItem
    {
        public int CreateYear { get; init; }
        public int CreateMonth { get; init; }
        public int? AssignedYear { get; init; }
        public int? AssignedMonth { get; init; }
        public int? DaysToAssigment { get; init; }
        public int? ClosedYear { get; init; }
        public int? ClosedMonth { get; init; }
        public int? DaysToClose { get; init; }
        public int? TotalDays { get; init; }
        public string Title { get; init; }
        public int Count { get; init; }
    }
}
