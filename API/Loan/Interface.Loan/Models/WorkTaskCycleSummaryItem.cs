namespace JestersCreditUnion.Interface.Loan.Models
{
    public class WorkTaskCycleSummaryItem
    {
        public int CreateYear { get; set; }
        public int CreateMonth { get; set; }
        public int? AssignedYear { get; set; }
        public int? AssignedMonth { get; set; }
        public int? DaysToAssigment { get; set; }
        public int? ClosedYear { get; set; }
        public int? ClosedMonth { get; set; }
        public int? DaysToClose { get; set; }
        public int? TotalDays { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
    }
}
