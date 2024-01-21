using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace JCU.Internal.ViewModel
{
    public class LoanApplicationRatingLogVM : ViewModelBase
    {
        private readonly LoanApplicationVM _loanApplicationVM;
        private FlowDocument _document;

        public LoanApplicationRatingLogVM(LoanApplicationVM loanApplicationVM)
        {
            _loanApplicationVM = loanApplicationVM;
        }

        public LoanApplicationVM LoanApplication => _loanApplicationVM;

        public FlowDocument Document
        {
            get => _document;
            set
            {
                if (_document != value)
                {
                    _document = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
