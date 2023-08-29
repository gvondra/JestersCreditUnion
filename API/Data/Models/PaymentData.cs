using System.Collections.Generic;

namespace JestersCreditUnion.Data.Models
{
    public class PaymentData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid PaymentId { get; set; }
        [ColumnMapping] public string LoanNumber { get; set; }
        [ColumnMapping] public string TransactionNumber { get; set; }
        [ColumnMapping] public DateTime Date { get; set; }
        [ColumnMapping] public decimal Amount { get; set; }
        [ColumnMapping] public short Status { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime UpdateTimestamp { get; set; }
        public List<TransactionData> Transactions { get; set; }
    }
}
