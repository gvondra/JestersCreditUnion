using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class LoanApplicationDenialLoader
    {
        private readonly LoanApplicationDenialVM _loanApplicationDenialVM;

        public LoanApplicationDenialLoader(LoanApplicationDenialVM loanApplicationDenialVM)
        {
            _loanApplicationDenialVM = loanApplicationDenialVM;
        }

        public void LoadUserName()
        {
            if (!_loanApplicationDenialVM.UserId.HasValue && string.IsNullOrEmpty(_loanApplicationDenialVM.UserName))
            {
                Task.Run(() =>
                {
                    using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                    {
                        ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                        IUserService userService = scope.Resolve<IUserService>();
                        return userService.Get(settingsFactory.CreateApi()).Result.Name;
                    }
                })
                    .ContinueWith(LoadUserNameCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private async Task LoadUserNameCallback(Task<string> loadUserName, object state)
        {
            try
            {
                _loanApplicationDenialVM.UserName = await loadUserName;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }

        public void LoadReasonsLookup()
        {
            Task.Run(() =>
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                    ILookupService lookupService = scope.Resolve<ILookupService>();
                    return lookupService.Get(settingsFactory.CreateLoanApi(), "loan-app-denial-reason").Result;
                }
            })
                .ContinueWith(LoadReasonsLookupCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadReasonsLookupCallback(Task<Lookup> load, object state)
        {
            try
            {
                _loanApplicationDenialVM.ReasonLookup = LookupVM.Create(await load);
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
