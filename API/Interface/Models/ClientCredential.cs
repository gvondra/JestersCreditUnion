using System;

namespace JestersCreditUnion.Interface.Models
{
    public class ClientCredential
    {
        public Guid? ClientId { get; set; }

        public string Secret { get; set; }
    }
}
