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
    public class LoanSearch : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter is LoanSearchVM loanSearchVM)
            {
                loanSearchVM.Loans.Clear();
                if (!loanSearchVM.HasErrors 
                    && (!string.IsNullOrEmpty(loanSearchVM.Number) || !string.IsNullOrEmpty(loanSearchVM.BorrowerName) || loanSearchVM.BorrowerBirthDate.HasValue))
                {
                    _canExecute = false;
                    CanExecuteChanged.Invoke(this, new EventArgs());
                    if (!string.IsNullOrEmpty(loanSearchVM.Number))
                    {
                        loanSearchVM.BorrowerName = string.Empty;
                        loanSearchVM.BorrowerBirthDate = null;
                    }
                    Task.Run(() => Search(loanSearchVM))
                        .ContinueWith(SearchCallback, loanSearchVM, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private List<Loan> Search(LoanSearchVM loanSearchVM)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ISettings settings = settingsFactory.CreateLoanApi();
                ILoanService loanService = scope.Resolve<ILoanService>();
                List<Loan> result = new List<Loan>();
                Loan loan;
                if (!string.IsNullOrEmpty(loanSearchVM.Number))
                {                    
                    loan = loanService.GetByNumber(settings, loanSearchVM.Number).Result;
                    if (loan != null)
                        result.Add(loan);
                }
                else if (!string.IsNullOrEmpty(loanSearchVM.BorrowerName) && loanSearchVM.BorrowerBirthDate.HasValue)
                {
                    result = result.Concat(
                        loanService.GetByBorrowerNameBirthDate(settings, loanSearchVM.BorrowerName, loanSearchVM.BorrowerBirthDate.Value).Result)
                        .ToList();
                }
                return result;
            }
        }

        private async Task SearchCallback(Task<List<Loan>> search, object state)
        {
            try
            {
                LoanSearchVM loanSearchVM = (LoanSearchVM)state;
                loanSearchVM.Loans.Clear();
                foreach (Loan loan in await search)
                {
                    loanSearchVM.Loans.Add(
                        LoanVM.Create(loan));
                }
            }
            catch (System.Exception ex)
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
