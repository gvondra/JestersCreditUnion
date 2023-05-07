using JCU.Internal.ViewModel;

namespace JCU.Internal.Behaviors
{
    public class LoanApplicationDenialValidator
    {
        private readonly LoanApplicationDenialVM _loanApplicationDenialVM;

        public LoanApplicationDenialValidator(LoanApplicationDenialVM loanApplicationDenialVM)
        {
            _loanApplicationDenialVM = loanApplicationDenialVM;
            _loanApplicationDenialVM.PropertyChanged += LoanApplicationDenialVM_PropertyChanged;
        }

        private void LoanApplicationDenialVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_loanApplicationDenialVM[e.PropertyName] != null)
                _loanApplicationDenialVM[e.PropertyName] = null;
            switch (e.PropertyName)
            {
                case nameof(LoanApplicationDenialVM.Text):
                    RequiredTextField(e.PropertyName, _loanApplicationDenialVM.Text);
                    break;
            }
        }

        private void RequiredTextField(string propertyName, string value)
        {
            if (string.IsNullOrEmpty(value))
                _loanApplicationDenialVM[propertyName] = "Is required";
        }
    }
}
