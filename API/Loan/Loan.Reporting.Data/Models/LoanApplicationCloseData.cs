namespace JestersCreditUnion.Loan.Reporting.Data.Models
{
    public class LoanApplicationCloseData
    {
        [ColumnMapping] public short ClosedYear { get; set; }
        [ColumnMapping] public short ClosedMonth { get; set; }
        [ColumnMapping] public string StatusDescription { get; set; }
        [ColumnMapping] public int Count { get; set; }
    }
}
