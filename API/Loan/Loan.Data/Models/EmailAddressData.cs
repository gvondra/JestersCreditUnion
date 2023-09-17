﻿namespace JestersCreditUnion.Loan.Data.Models
{
    public class EmailAddressData : DataManagedStateBase
    {
        [ColumnMapping(IsPrimaryKey = true)] public Guid EmailAddressId { get; set; }
        [ColumnMapping()] public string Address { get; set; }
        [ColumnMapping(IsUtc = true)] public DateTime CreateTimestamp { get; set; }
    }
}