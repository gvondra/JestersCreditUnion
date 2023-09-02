namespace JestersCreditUnion.Data.Models
{
    public class PaymentTransactionData : TransactionData
    {
        [ColumnMapping] public Guid PaymentId { get; set; }
        [ColumnMapping] public short TermNumber { get; set; }
    }
}
