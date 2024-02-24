using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class InterestRateConfigurationSaver : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IInterestRateConfigurationService _interestRateConfigurationService;
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public InterestRateConfigurationSaver(ISettingsFactory settingsFactory, IInterestRateConfigurationService interestRateConfigurationService)
        {
            _settingsFactory = settingsFactory;
            _interestRateConfigurationService = interestRateConfigurationService;
        }

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter is InterestRateConfigurationVM interestRateConfigurationVM)
            {
                _canExecute = false;
                CanExecuteChanged?.Invoke(this, new EventArgs());
                Task.Run(() => Save(interestRateConfigurationVM))
                    .ContinueWith(SaveCallback, interestRateConfigurationVM, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private InterestRateConfiguration Save(InterestRateConfigurationVM interestRateConfigurationVM)
        {
            InterestRateConfiguration request = new InterestRateConfiguration
            {
                InflationRate = interestRateConfigurationVM.InflationRate,
                OperationsRate = interestRateConfigurationVM.OperationsRate,
                LossRate = interestRateConfigurationVM.LossRate,
                IncentiveRate = interestRateConfigurationVM.IncentiveRate,
                OtherRate = interestRateConfigurationVM.OtherRate,
                TotalRate = interestRateConfigurationVM.TotalRate,
                MinimumRate = interestRateConfigurationVM.MinimumRate,
                MaximumRate = interestRateConfigurationVM.MaximumRate,
                OtherRateDescription = interestRateConfigurationVM.OtherRateDescription
            };
            return _interestRateConfigurationService.Save(_settingsFactory.CreateLoanApi(), request).Result;
        }

        private async Task SaveCallback(Task<InterestRateConfiguration> get, object state)
        {
            try
            {
                InterestRateConfiguration interestRateConfiguration = await get;
                if (state != null && state is InterestRateConfigurationVM interestRateConfigurationVM)
                {
                    interestRateConfigurationVM.LoadConfiguration(interestRateConfiguration);
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _canExecute = true;
                CanExecuteChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
