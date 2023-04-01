using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Framework
{
    public interface ILoanApplicationSaver
    {
        Task Create(ISettings settings, ILoanApplication loanApplication);
    }
}
