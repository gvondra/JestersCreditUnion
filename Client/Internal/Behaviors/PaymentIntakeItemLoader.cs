using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class PaymentIntakeItemLoader
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly ILoanService _loanService;
        private readonly PaymentIntakeItemVM _paymentIntakeItemVM;

        public PaymentIntakeItemLoader(ISettingsFactory settingsFactory, ILoanService loanService, PaymentIntakeItemVM paymentIntakeItemVM)
        {
            _settingsFactory = settingsFactory;
            _loanService = loanService;
            _paymentIntakeItemVM = paymentIntakeItemVM;
            _paymentIntakeItemVM.PropertyChanged += PaymentIntakeItemVM_PropertyChanged;
        }

        private void PaymentIntakeItemVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_paymentIntakeItemVM[e.PropertyName] != null)
                _paymentIntakeItemVM[e.PropertyName] = null;
            switch (e.PropertyName)
            {
                case nameof(PaymentIntakeItemVM.Date):
                    ValidateDate();
                    break;
                case nameof(PaymentIntakeItemVM.LoanNumber):
                    LoanNumberChanged();
                    break;
            }
        }

        private void ValidateDate()
        {
            if (!_paymentIntakeItemVM.Date.HasValue)
                _paymentIntakeItemVM[nameof(PaymentIntakeItemVM.Date)] = "is required";
            else if (DateTime.Today < _paymentIntakeItemVM.Date.Value || _paymentIntakeItemVM.Date.Value < DateTime.Today.AddYears(-100))
                _paymentIntakeItemVM[nameof(PaymentIntakeItemVM.Date)] = "is invalid";
            _paymentIntakeItemVM.CanAdd = !_paymentIntakeItemVM.HasErrors;
        }

        private void LoanNumberChanged()
        {
            _paymentIntakeItemVM.CanAdd = false;
            _paymentIntakeItemVM.LoanNumberTip = "Searching for Loan";
            if (!string.IsNullOrEmpty(_paymentIntakeItemVM.LoanNumber))
            {
                Task.Run(() => _loanService.GetByNumber(_settingsFactory.CreateLoanApi(), _paymentIntakeItemVM.LoanNumber))
                    .ContinueWith(GetLoanCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private async Task GetLoanCallback(Task<Loan> getLoan, object state)
        {
            try
            {
                Loan loan = await getLoan;
                if (loan != null)
                {
                    _paymentIntakeItemVM[nameof(PaymentIntakeItemVM.LoanNumber)] = null;
                    _paymentIntakeItemVM.LoanNumberTip = string.Empty;
                    _paymentIntakeItemVM.NextPaymentDue = loan.NextPaymentDue;
                }
                else
                {
                    _paymentIntakeItemVM.NextPaymentDue = null;
                    _paymentIntakeItemVM[nameof(PaymentIntakeItemVM.LoanNumber)] = "Loan Not Found";
                    _paymentIntakeItemVM.LoanNumberTip = "Loan Not Found";
                }
                _paymentIntakeItemVM.CanAdd = !_paymentIntakeItemVM.HasErrors;
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
