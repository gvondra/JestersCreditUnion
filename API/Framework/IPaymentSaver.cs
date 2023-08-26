using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IPaymentSaver
    {
        Task<IEnumerable<IPayment>> Save(ISettings settings, IEnumerable<IPayment> payments);
    }
}
