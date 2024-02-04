using JCU.Internal.ViewModel;
using System;

namespace JCU.Internal.Behaviors
{
    public class BeginLoanAgreementValidator
    {
        private readonly BeginLoanAgreementVM _beginLoanAgreementVM;

        public BeginLoanAgreementValidator(BeginLoanAgreementVM beginLoanAgreementVM)
        {
            _beginLoanAgreementVM = beginLoanAgreementVM;
            beginLoanAgreementVM.Loan.Agreement.PropertyChanged += Agreement_PropertyChanged;
            ValidateRangeExclusive(nameof(LoanAgreementVM.InterestPercentage), _beginLoanAgreementVM.Loan.Agreement.InterestPercentage, _beginLoanAgreementVM.Loan.Agreement, 0.0M, 100.0M);
        }

        private void Agreement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_beginLoanAgreementVM.Loan.Agreement[e.PropertyName] != null)
                _beginLoanAgreementVM.Loan.Agreement[e.PropertyName] = null;
            switch (e.PropertyName)
            {
                case nameof(LoanAgreementVM.BorrowerName):
                    RequiredTextField(e.PropertyName, _beginLoanAgreementVM.Loan.Agreement.BorrowerName, _beginLoanAgreementVM.Loan.Agreement);
                    break;
                case nameof(LoanAgreementVM.BorrowerBirthDate):
                    RequiredNullableField(e.PropertyName, _beginLoanAgreementVM.Loan.Agreement.BorrowerBirthDate, _beginLoanAgreementVM.Loan.Agreement);
                    break;
                case nameof(LoanAgreementVM.InterestPercentage):
                    ValidateRangeExclusive(e.PropertyName, _beginLoanAgreementVM.Loan.Agreement.InterestPercentage, _beginLoanAgreementVM.Loan.Agreement, 0.0M, 100.0M);
                    break;
                case nameof(LoanAgreementVM.OriginalAmount):
                    RequiredNullableField(e.PropertyName, _beginLoanAgreementVM.Loan.Agreement.OriginalAmount, _beginLoanAgreementVM.Loan.Agreement);
                    ValidateRangeExclusive(e.PropertyName, _beginLoanAgreementVM.Loan.Agreement.OriginalAmount, _beginLoanAgreementVM.Loan.Agreement, 0.0M, null);
                    break;
                case nameof(LoanAgreementVM.OriginalTerm):
                    RequiredNullableField(e.PropertyName, _beginLoanAgreementVM.Loan.Agreement.OriginalTerm, _beginLoanAgreementVM.Loan.Agreement);
                    ValidateRangeExclusive(e.PropertyName, _beginLoanAgreementVM.Loan.Agreement.OriginalTerm, _beginLoanAgreementVM.Loan.Agreement, 0, null);
                    break;
            }
        }

        private void RequiredTextField(string propertyName, string value, ViewModelBase viewModel)
        {
            if (string.IsNullOrEmpty(value))
                viewModel[propertyName] = "Is required";
        }

        private void RequiredNullableField<T>(string propertyName, Nullable<T> value, ViewModelBase viewModel)
            where T : struct
        {
            if (!value.HasValue)
                viewModel[propertyName] = "Is required";
        }

        private void ValidateRangeExclusive(string propertyName, decimal? value, ViewModelBase viewModel, decimal? minValue, decimal? maxValue)
        {
            if (value.HasValue)
            {
                if (minValue.HasValue && maxValue.HasValue)
                {
                    if (value.Value <= minValue.Value || value.Value >= maxValue.Value)
                    {
                        viewModel[propertyName] = $"Must be between {minValue} and {maxValue}";
                    }
                }
                else if (minValue.HasValue)
                {
                    if (value.Value <= minValue.Value)
                    {
                        viewModel[propertyName] = $"Must be greater than {minValue}";
                    }
                }
                else if (maxValue.HasValue)
                {
                    if (value.Value >= maxValue.Value)
                    {
                        viewModel[propertyName] = $"Must be greater than {maxValue}";
                    }
                }
            }
        }

        private void ValidateRangeExclusive(string propertyName, short? value, ViewModelBase viewModel, short? minValue, short? maxValue)
        {
            if (value.HasValue)
            {
                if (minValue.HasValue && maxValue.HasValue)
                {
                    if (value.Value <= minValue.Value || value.Value >= maxValue.Value)
                    {
                        viewModel[propertyName] = $"Must be between {minValue} and {maxValue}";
                    }
                }
                else if (minValue.HasValue)
                {
                    if (value.Value <= minValue.Value)
                    {
                        viewModel[propertyName] = $"Must be greater than {minValue}";
                    }
                }
                else if (maxValue.HasValue)
                {
                    if (value.Value >= maxValue.Value)
                    {
                        viewModel[propertyName] = $"Must be greater than {maxValue}";
                    }
                }
            }
        }
    }
}
