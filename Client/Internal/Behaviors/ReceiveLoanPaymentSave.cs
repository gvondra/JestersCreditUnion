using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class ReceiveLoanPaymentSave : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter is ReceiveLoanPaymentVM receiveLoanPaymentVM)
            {
                if (!receiveLoanPaymentVM.Amount.HasValue)
                {
                    receiveLoanPaymentVM[nameof(ReceiveLoanPaymentVM.Amount)] = "Is required";
                }
                else if (!receiveLoanPaymentVM.HasErrors)
                {
                    receiveLoanPaymentVM.BusyVisibility = Visibility.Visible;
                    receiveLoanPaymentVM.SaveEnabled = false;
                    Task.Run(() => Save(receiveLoanPaymentVM))
                        .ContinueWith(SaveCallback, receiveLoanPaymentVM, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
            else
            {
                throw new ApplicationException($"Invalid parameter of type {parameter.GetType().FullName}");
            }
        }

        private static LoanPayment Save(ReceiveLoanPaymentVM receiveLoanPaymentVM)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ILoanPaymentService service = scope.Resolve<ILoanPaymentService>();
                List<LoanPayment> result = service.Save(
                    settingsFactory.CreateLoanApi(),
                    new List<LoanPayment> { receiveLoanPaymentVM.InnerLoanPayment })
                    .Result;
                return result.SingleOrDefault();
            }
        }

        private async Task SaveCallback(Task<LoanPayment> save, object state)
        {
            try
            {
                if (state != null && state is ReceiveLoanPaymentVM receiveLoanPaymentVM)
                {
                    LoanPayment loanPayment = await save;
                    receiveLoanPaymentVM.Message = loanPayment.Message;
                    receiveLoanPaymentVM.BusyVisibility = Visibility.Collapsed;
                    receiveLoanPaymentVM.SaveEnabled = false;
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
