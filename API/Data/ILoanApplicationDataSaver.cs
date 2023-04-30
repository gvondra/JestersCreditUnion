using JestersCreditUnion.Data.Models;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Data
{
    public interface ILoanApplicationDataSaver
    {
        Task Create(IDataSettings settings, LoanApplicationData data);
        Task Update(IDataSettings settings, LoanApplicationData data);
        Task AppendComment(IDataSettings settings, Guid id, LoanApplicationCommentData data);
    }
}
