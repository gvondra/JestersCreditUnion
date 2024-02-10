using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class PaymentIntakeAdd : ICommand
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
                _canExecute = false;
                CanExecuteChanged.Invoke(this, new EventArgs());
                PaymentIntakeItemVM newItem = new PaymentIntakeItemVM(
                    paymentIntakeItemVM.PaymentIntakeVM,
                    new PaymentIntake())
                {
                    Add = new PaymentIntakeAdd()
                };
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    Behaviors.PaymentIntakeItemLoader loader = scope.Resolve<Func<ViewModel.PaymentIntakeItemVM, Behaviors.PaymentIntakeItemLoader>>()(newItem);
                    newItem.AddBehavior(loader);
                }   
                newItem.Date = paymentIntakeItemVM.Date;
                newItem.CanAdd = false;
                paymentIntakeItemVM.PaymentIntakeVM.NewItem = newItem;
                paymentIntakeItemVM.PaymentIntakeVM.Items.Add(paymentIntakeItemVM);
            }
        }
    }
}
