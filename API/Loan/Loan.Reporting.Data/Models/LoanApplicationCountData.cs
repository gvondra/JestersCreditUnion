namespace JestersCreditUnion.Loan.Reporting.Data.Models
{
    public class LoanApplicationCountData
    {
        [ColumnMapping] public short ApplicationYear { get; set; }
        [ColumnMapping] public short ApplicationMonth { get; set; }
        [ColumnMapping] public int Count { get; set; }
    }
}
