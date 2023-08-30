using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface IAmortizationBuilder
    {
        Task<IEnumerable<IAmortizationItem>> Build(ISettings settings, ILoan loan);
    }
}
