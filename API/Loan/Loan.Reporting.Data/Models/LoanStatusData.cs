namespace JestersCreditUnion.Loan.Reporting.Data.Models
{
    public class LoanStatusData
    {
        [ColumnMapping("Status", IsPrimaryKey = true)] public short Status { get; set; }
        [ColumnMapping("Description")] public string Description { get; set; }
    }
}
