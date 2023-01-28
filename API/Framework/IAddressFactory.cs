using JestersCreditUnion.CommonCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IAddressFactory
    {
        IAddress Create(
            string recipient,
            string attention,
            string delivery,
            string secondary,
            string city,
            ref string state,
            ref string postalCode
            );
        IAddress Create(
            string recipient,
            string delivery,
            string city,
            ref string state,
            ref string postalCode
            );
        Task<IAddress> Get(ISettings settings, Guid id);
        Task<IAddress> GetByHash(ISettings settings, byte[] hash);
    }
}
