using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class IdentificationCardSaver : IIdentificationCardSaver
    {
        private readonly LoanApplication _loanApplication;

        internal IdentificationCardSaver(LoanApplication loanApplication)
        {
            _loanApplication = loanApplication;
        }

        public IBlob Blob { get; set; } = new Blob();

        public async Task SaveBorrowerIdentificationCard(ISettings settings)
        {
            if (string.IsNullOrEmpty(_loanApplication.BorrowerIdentificationCardName))
            {
                _loanApplication.BorrowerIdentificationCardName = Guid.NewGuid().ToString("N");
                await CommonCore.Saver.Save(new CommonCore.TransactionHandler(settings), (th) => _loanApplication.Update(th));
            }
            await Blob.Upload(settings, _loanApplication.BorrowerIdentificationCardName);
        }
    }
}
