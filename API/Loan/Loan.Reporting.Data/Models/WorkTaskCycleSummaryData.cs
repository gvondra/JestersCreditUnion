namespace JestersCreditUnion.Loan.Reporting.Data.Models
{
    public class WorkTaskCycleSummaryData
    {
        [ColumnMapping] public int CreateYear { get; set; }
        [ColumnMapping] public int CreateMonth { get; set; }
        [ColumnMapping] public int? AssignedYear { get; set; }
        [ColumnMapping] public int? AssignedMonth { get; set; }
        [ColumnMapping] public int? DaysToAssigment { get; set; }
        [ColumnMapping] public int? ClosedYear { get; set; }
        [ColumnMapping] public int? ClosedMonth { get; set; }
        [ColumnMapping] public int? DaysToClose { get; set; }
        [ColumnMapping] public int? TotalDays { get; set; }
        [ColumnMapping] public string Title { get; set; }
        [ColumnMapping] public int Count { get; set; }
    }
}
