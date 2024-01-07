namespace JestersCreditUnion.Loan.Reporting.Data.Models
{
    public class LoanApplicationCountData
    {
        [ColumnMapping] public int ApplicationYear { get; set; }
        [ColumnMapping] public int ApplicationMonth { get; set; }
        [ColumnMapping] public int Count { get; set; }
    }
}
