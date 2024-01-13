using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IWorkTaskTypeCodeLookup
    {
        Task<string> GetNewLoanApplicationWorkTaskTypeCode(ISettings settings);
        Task<string> GetSendDeinalCorrespondenceWorkTaskTypeCode(ISettings settings);
        Task<string> GetDiburseFundsTaskTypeCode(ISettings settings);
        Task<string> GetWorkTaskTypeCode(ISettings settings, string fieldName);
    }
}
