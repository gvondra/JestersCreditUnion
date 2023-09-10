using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IPhoneFactory
    {
        IPhone Create(ref string number);
        Task<IPhone> Get(ISettings settings, Guid id);
        Task<IPhone> Get(ISettings settings, string number);
    }
}
