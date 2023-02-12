using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JestersCreditUnion.Data.Models
{
    public class EmailAddressData 
    {
        private string _address;

        [BsonId()] 
        public Guid EmailAddressId { get; set; }
        [BsonRequired()]
        public string Address 
        { 
            get => _address ?? string.Empty; 
            set
            {
                _address = (value ?? string.Empty).Trim();
                AddressLCase = _address.ToLower();
            }
        }
        [BsonRequired()]
        public string AddressLCase { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)] 
        public DateTime CreateTimestamp { get; set; }
    }
}
