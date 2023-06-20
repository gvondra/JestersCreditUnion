using JestersCreditUnion.Framework;
using System.Threading.Tasks;
using CommonCore = JestersCreditUnion.CommonCore;

namespace JestersCreditUnion.Core
{
    public class LoanSaver : ILoanSaver
    {
        public Task Create(ISettings settings, ILoan loan)
        {
            return CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), loan.Create);
        }

        public Task Update(ISettings settings, ILoan loan)
        {
            return CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), loan.Update);
        }
    }
}
