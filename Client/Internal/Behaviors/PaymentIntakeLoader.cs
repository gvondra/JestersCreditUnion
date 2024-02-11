using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class PaymentIntakeLoader : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IPaymentIntakeService _paymentIntakeService;
        private bool _canExecute = true;

        public PaymentIntakeLoader(
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
            if (parameter is PaymentIntakeVM paymentIntakeVM)
            {
                Task.Run(() => _paymentIntakeService.GetByStatuses(_settingsFactory.CreateLoanApi(), new short[] { 0, 1 }).Result)
                    .ContinueWith(GetCallback, paymentIntakeVM, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private async Task GetCallback(Task<List<PaymentIntake>> get, object state)
        {
            try
            {
                List<PaymentIntake> paymentIntakes = await get;
                if (state is PaymentIntakeVM paymentIntakeVM)
                {
                    using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                    {
                        Dictionary<Guid, PaymentIntakeItemVM> itemLookup = paymentIntakeVM.Items
                            .ToDictionary(vm => vm.PaymentIntakeId.Value);
                        foreach (PaymentIntake paymentIntake in paymentIntakes)
                        {
                            PaymentIntakeItemVM existingItem;
                            if (itemLookup.TryGetValue(paymentIntake.PaymentIntakeId.Value, out existingItem))
                            {
                                existingItem.StatusDescription = paymentIntake.StatusDescription;
                                existingItem.UpdateUserName = paymentIntake.UpdateUserName;
                                existingItem.UpdateTimestamp = paymentIntake.UpdateTimestamp;
                            }
                            else
                            {
                                PaymentIntakeItemVM paymentIntakeItemVM = new PaymentIntakeItemVM(paymentIntakeVM, paymentIntake);
                                PaymentIntakeItemLoader loader = scope.Resolve<Func<ViewModel.PaymentIntakeItemVM, Behaviors.PaymentIntakeItemLoader>>()(paymentIntakeItemVM);
                                paymentIntakeItemVM.AddBehavior(loader);
                                paymentIntakeItemVM.Update = scope.Resolve<Behaviors.PaymentIntakeItemUpdater>();

                                paymentIntakeVM.Items.Add(paymentIntakeItemVM);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
