using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class LoanApplicationVM : ViewModelBase
    {
        private readonly LoanApplication _loanApplication;
        private Visibility _busyVisibility = Visibility.Collapsed;

        private LoanApplicationVM(LoanApplication loanApplication)
        {
            _loanApplication = loanApplication;
        }

        public Visibility BusyVisibility 
        {
            get => _busyVisibility;
            set
            {
                if (_busyVisibility != value)
                {
                    _busyVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static LoanApplicationVM Create(LoanApplication loanApplication)
        {
            LoanApplicationVM vm = new LoanApplicationVM(loanApplication);
            return vm;
        }
    }
}
