using JestersCreditUnion.Framework;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class LoanSaver : ILoanSaver
    {
        public Task Create(ISettings settings, ILoan loan)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), loan.Create);

        public Task Update(ISettings settings, ILoan loan)
            => CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), loan.Update);
    }
}
