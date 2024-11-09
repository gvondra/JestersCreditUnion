namespace JestersCreditUnion.Loan.Data.Models
{
    public class LoanApplicationCommentData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid LoanApplicationCommentId { get; set; }
        [ColumnMapping] public Guid LoanApplicationId { get; set; }
        [ColumnMapping] public Guid UserId { get; set; }
        [ColumnMapping] public bool IsInternal { get; set; } = true;
        [ColumnMapping] public string Text { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
    }
}
