using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class PaymentIntakeAdd : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IPaymentIntakeService _paymentIntakeService;
        private bool _canExecute = true;

        public PaymentIntakeAdd(
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
                _canExecute = false;
                CanExecuteChanged.Invoke(this, new EventArgs());
                Task.Run(() => CreatePaymentIntake(paymentIntakeItemVM.InnerPaymentIntake))
                    .ContinueWith(CreateCallback, paymentIntakeItemVM, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private PaymentIntake CreatePaymentIntake(PaymentIntake paymentIntake)
            => _paymentIntakeService.Create(_settingsFactory.CreateLoanApi(), paymentIntake).Result;

        private async Task CreateCallback(Task<PaymentIntake> create, object state)
        {
            try
            {
                PaymentIntake paymentIntake = await create;
                if (state != null && state is PaymentIntakeItemVM paymentIntakeItemVM)
                {
                    PaymentIntakeItemVM newItem = new PaymentIntakeItemVM(
                    paymentIntakeItemVM.PaymentIntakeVM,
                    new PaymentIntake())
                    {
                        Date = paymentIntakeItemVM.Date,
                        CanAdd = false
                    };
                    using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                    {
                        Behaviors.PaymentIntakeItemLoader loader = scope.Resolve<Func<ViewModel.PaymentIntakeItemVM, Behaviors.PaymentIntakeItemLoader>>()(newItem);
                        newItem.AddBehavior(loader);
                        newItem.Add = scope.Resolve<PaymentIntakeAdd>();
                    }
                    paymentIntakeItemVM.PaymentIntakeVM.NewItem = newItem;

                    paymentIntakeItemVM.PaymentIntakeVM.Load?.Execute(paymentIntakeItemVM.PaymentIntakeVM);
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
