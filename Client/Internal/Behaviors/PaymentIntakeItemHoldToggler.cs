using JCU.Internal.ViewModel;
using System;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class PaymentIntakeItemHoldToggler : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter is PaymentIntakeItemVM paymentIntakeItemVM)
            {
                paymentIntakeItemVM.InnerPaymentIntake.Status = paymentIntakeItemVM.InnerPaymentIntake.Status == 0 ? (short)1 : (short)0;
                paymentIntakeItemVM.Update?.Execute(paymentIntakeItemVM);
            }
        }
    }
}
