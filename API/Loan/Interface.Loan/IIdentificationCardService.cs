using System;
using System.IO;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public interface IIdentificationCardService
    {
        Task<Stream> GetBorrowerIdentificationCard(ISettings settings, Guid loanApplicationId);
    }
}
