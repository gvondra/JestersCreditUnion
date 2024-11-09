using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class PaymentIntakeItemUpdater : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IPaymentIntakeService _paymentIntakeService;
        private bool _canExecute = true;

        public PaymentIntakeItemUpdater(
            ISettingsFactory settingsFactory,
            IPaymentIntakeService paymentIntakeService)
        {
            _settingsFactory = settingsFactory;
            _paymentIntakeService = paymentIntakeService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter is PaymentIntakeItemVM paymentIntakeItemVM)
            {
                Task.Run(() => Update(paymentIntakeItemVM.InnerPaymentIntake))
                    .ContinueWith(UpdateCallback, paymentIntakeItemVM, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private PaymentIntake Update(PaymentIntake paymentIntake)
        {
            return _paymentIntakeService.Update(_settingsFactory.CreateLoanApi(), paymentIntake).Result;
        }

        private async Task UpdateCallback(Task<PaymentIntake> update, object state)
        {
            try
            {
                PaymentIntake payment = await update;
                if (state != null && state is PaymentIntakeItemVM paymentIntakeItemVM)
                {
                    paymentIntakeItemVM.StatusDescription = payment.StatusDescription;
                    paymentIntakeItemVM.UpdateUserName = payment.UpdateUserName;
                    paymentIntakeItemVM.UpdateTimestamp = payment.UpdateTimestamp;
                }
                MessageBox.Show("Item updated", "Update Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
