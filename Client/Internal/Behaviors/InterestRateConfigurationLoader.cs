using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class InterestRateConfigurationLoader : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IInterestRateConfigurationService _interestRateConfigurationService;
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public InterestRateConfigurationLoader(ISettingsFactory settingsFactory, IInterestRateConfigurationService interestRateConfigurationService)
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
                interestRateConfigurationVM.IsLoading = true;
                Task.Run(() => _interestRateConfigurationService.Get(_settingsFactory.CreateLoanApi()).Result)
                    .ContinueWith(GetCallback, interestRateConfigurationVM, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private async Task GetCallback(Task<InterestRateConfiguration> get, object state)
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
                if (state != null && state is InterestRateConfigurationVM interestRateConfigurationVM)
                {
                    interestRateConfigurationVM.IsLoading = false;
                }
            }
        }
    }
}
