using JCU.Internal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class LoanApplicationValidator
    {
        private readonly LoanApplicationVM _loanApplicationVM;

        public LoanApplicationValidator(LoanApplicationVM loanApplicationVM)
        {
            _loanApplicationVM = loanApplicationVM;
            _loanApplicationVM.PropertyChanged += LoanApplicationVM_PropertyChanged;
        }

        private void LoanApplicationVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_loanApplicationVM[e.PropertyName] != null) 
                _loanApplicationVM[e.PropertyName] = null;
            switch (e.PropertyName)
            {
                case nameof(LoanApplicationVM.ApplicationDate):
                    RequiredDateField(e.PropertyName, _loanApplicationVM.ApplicationDate);
                    DateRange(e.PropertyName, _loanApplicationVM.ApplicationDate, null, DateTime.Today);
                    break;
                case nameof(LoanApplicationVM.BorrowerName):
                    RequiredTextField(e.PropertyName, _loanApplicationVM.BorrowerName);
                    break;
                case nameof(LoanApplicationVM.BorrowerBirthDate):
                    RequiredDateField(e.PropertyName, _loanApplicationVM.BorrowerBirthDate);
                    DateRange(e.PropertyName, _loanApplicationVM.BorrowerBirthDate, null, DateTime.Today);
                    break;
                case nameof(LoanApplicationVM.Amount):
                    RequiredDecimalField(e.PropertyName, _loanApplicationVM.Amount);
                    break;
            }
        }

        private void RequiredTextField(string propertyName, string value)
        {
            if (string.IsNullOrEmpty(value))
                _loanApplicationVM[propertyName] = "Is required";
        }

        private void RequiredDateField(string propertyName, DateTime? value)
        {
            if (!value.HasValue)
                _loanApplicationVM[propertyName] = "Is required";
        }

        private void RequiredDecimalField(string propertyName, decimal? value)
        {
            if (!value.HasValue)
                _loanApplicationVM[propertyName] = "Is required";
        }

        private void DateRange(string propertyName, DateTime? value, DateTime? min, DateTime? max)
        {
            if (value.HasValue)
            {
                if (min.HasValue && max.HasValue 
                    && (value.Value < min.Value || max.Value < value.Value))
                {
                    _loanApplicationVM[propertyName] = $"Is outside the range {min.Value.ToString("yyyy-MM-dd")} through {max.Value.ToString("yyyy-MM-dd")}";
                }
                else if (min.HasValue && value.Value < min.Value)
                {
                    _loanApplicationVM[propertyName] = $"Is less than the minimum of {min.Value.ToString("yyyy-MM-dd")}";
                }
                else if (max.HasValue && max.Value < value.Value)
                {
                    _loanApplicationVM[propertyName] = $"Is greater than the meximum of {max.Value.ToString("yyyy-MM-dd")}";
                }

            }
        }
    }
}
