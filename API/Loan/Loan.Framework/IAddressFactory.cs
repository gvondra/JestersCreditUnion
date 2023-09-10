using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
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
