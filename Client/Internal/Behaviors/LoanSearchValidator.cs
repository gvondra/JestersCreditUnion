using JCU.Internal.ViewModel;
using System;
using System.Text.RegularExpressions;

namespace JCU.Internal.Behaviors
{
    public class LoanSearchValidator
    {
        private readonly LoanSearchVM _loanSearchVM;

        public LoanSearchValidator(LoanSearchVM loanSearchVM)
        {
            _loanSearchVM = loanSearchVM;
            _loanSearchVM.PropertyChanged += LoanSearchVM_PropertyChanged;
        }

        private void LoanSearchVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_loanSearchVM[e.PropertyName] != null)
                _loanSearchVM[e.PropertyName] = null;
            switch (e.PropertyName)
            {
                case nameof(LoanSearchVM.Number):
                    ValidateLoanNumber(e.PropertyName, _loanSearchVM.Number, _loanSearchVM);
                    break;
                case nameof(LoanSearchVM.BorrowerBirthDate):
                    ValidateBirthDate(e.PropertyName, _loanSearchVM.BorrowerBirthDate, _loanSearchVM);
                    break;
            }
        }

        private void ValidateLoanNumber(string propertyName, string loanNumber, ViewModelBase viewModelBase)
        {
            if (!Regex.IsMatch(loanNumber, @"^\d+$", RegexOptions.IgnoreCase))
                viewModelBase[propertyName] = "Invalid";
        }

        private void ValidateBirthDate(string propertyName, DateTime? birthDate, ViewModelBase viewModelBase)
        {
            if (birthDate.HasValue)
            {
                if (DateTime.Today < birthDate.Value || birthDate.Value < DateTime.Today.AddYears(-150))
                {
                    viewModelBase[propertyName] = "Out of range";
                }
            }
        }
    }
}
