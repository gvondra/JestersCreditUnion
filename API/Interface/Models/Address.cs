using System;

namespace JestersCreditUnion.Interface.Models
{
    public class Address
    {
        public Guid? AddressId { get; set; }
        public string Recipient { get; set; }
        public string Attention { get; set; }
        public string Delivery { get; set; }
        public string Secondary { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public DateTime? CreateTimestamp { get; set; }
    }
}
