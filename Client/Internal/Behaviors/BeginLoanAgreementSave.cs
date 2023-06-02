﻿using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class BeginLoanAgreementSave : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter is BeginLoanAgreementVM beginLoanAgreementVM)
            {
                if (!beginLoanAgreementVM.HasErrors && !beginLoanAgreementVM.Loan.HasErrors && !beginLoanAgreementVM.Loan.Agreement.HasErrors)
                {
                    _canExecute = false;
                    CanExecuteChanged.Invoke(this, new EventArgs());
                    Task.Run(() => SaveLoan(beginLoanAgreementVM))
                        .ContinueWith(SaveLoanCallback, beginLoanAgreementVM, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        private Loan SaveLoan(BeginLoanAgreementVM beginLoanAgreementVM)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                ISettings settings = settingsFactory.CreateApi();
                ILoanService loanService = scope.Resolve<ILoanService>();
                Loan loan = beginLoanAgreementVM.Loan.InnerLoan;
                if (!loan.LoanId.HasValue)
                {
                    return loanService.Create(settings, loan).Result;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task SaveLoanCallback(Task<Loan> saveLoan, object state)
        {
            try
            {
                BeginLoanAgreementVM beginLoanAgreementVM = (BeginLoanAgreementVM)state;
                Loan loan = await saveLoan;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _canExecute = true;
                CanExecuteChanged.Invoke(this, new EventArgs());
            }
        }
    }
}