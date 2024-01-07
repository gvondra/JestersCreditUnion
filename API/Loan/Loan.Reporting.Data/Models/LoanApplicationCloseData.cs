namespace JestersCreditUnion.Loan.Reporting.Data.Models
{
    public class LoanApplicationCloseData
    {
        [ColumnMapping] public int ClosedYear { get; set; }
        [ColumnMapping] public int ClosedMonth { get; set; }
        [ColumnMapping] public string StatusDescription { get; set; }
        [ColumnMapping] public int Count { get; set; }
    }
}
