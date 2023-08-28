using JCU.Internal.ViewModel;
using System;

namespace JCU.Internal.Behaviors
{
    public class ReceiveLoanPaymentValidator
    {
        private readonly ReceiveLoanPaymentVM _receiveLoanPaymentVM;

        public ReceiveLoanPaymentValidator(ReceiveLoanPaymentVM receiveLoanPaymentVM)
        {
            _receiveLoanPaymentVM = receiveLoanPaymentVM;
            _receiveLoanPaymentVM.PropertyChanged += ReceiveLoanPaymentVM_PropertyChanged;
        }

        private void ReceiveLoanPaymentVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_receiveLoanPaymentVM[e.PropertyName] != null)
                _receiveLoanPaymentVM[e.PropertyName] = null;
            switch (e.PropertyName)
            {
                case nameof(ReceiveLoanPaymentVM.ReceivedDate):
                    ValidateReceivedDate(e.PropertyName, _receiveLoanPaymentVM.ReceivedDate);
                    break;
                case nameof(ReceiveLoanPaymentVM.Amount):
                    ValidateAmount(e.PropertyName, _receiveLoanPaymentVM.Amount);
                    break;
            }
            _receiveLoanPaymentVM.SaveEnabled = !_receiveLoanPaymentVM.HasErrors;
        }

        private void ValidateReceivedDate(string propertyName, DateTime receivedDate)
        {
            if (receivedDate < DateTime.Today.AddYears(-100))
                _receiveLoanPaymentVM[propertyName] = "Invalid date";
            else if (DateTime.Today < receivedDate)
                _receiveLoanPaymentVM[propertyName] = "Invalid future received date";
        }

        private void ValidateAmount(string propertyName, decimal? amount)
        {
            if (!amount.HasValue)
                _receiveLoanPaymentVM[propertyName] = "Is required";
            else if (amount.Value <= 0.0M)
                _receiveLoanPaymentVM[propertyName] = "Must be greater then zero";
        }
    }
}
