namespace JestersCreditUnion.Data.Models
{
    public class PaymentTransactionData : TransactionData
    {
        public Guid PaymentId { get; set; }
        public short TermNumber { get; set; }
    }
}
