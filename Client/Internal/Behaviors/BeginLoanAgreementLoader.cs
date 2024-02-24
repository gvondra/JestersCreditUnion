using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace JCU.Internal.Behaviors
{
    public class BeginLoanAgreementLoader
    {
        private readonly BeginLoanAgreementVM _beginLoanAgreementVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IInterestRateConfigurationService _interestRateConfigurationService;

        public BeginLoanAgreementLoader(
            ISettingsFactory settingsFactory,
            IInterestRateConfigurationService interestRateConfigurationService,
            BeginLoanAgreementVM beginLoanAgreementVM)
        {
            _settingsFactory = settingsFactory;
            _interestRateConfigurationService = interestRateConfigurationService;
            _beginLoanAgreementVM = beginLoanAgreementVM;
            beginLoanAgreementVM.Loan.Agreement.PropertyChanged += Agreement_PropertyChanged;
        }

        private void Agreement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_beginLoanAgreementVM.Loan.Agreement[e.PropertyName] != null)
                _beginLoanAgreementVM.Loan.Agreement[e.PropertyName] = null;
            switch (e.PropertyName)
            {
                case nameof(LoanAgreementVM.InterestPercentage):
                case nameof(LoanAgreementVM.OriginalAmount):
                case nameof(LoanAgreementVM.OriginalTerm):
                case nameof(LoanAgreementVM.PaymentFrequency):
                    Task.Run(() => CalculatePaymentAmount(_beginLoanAgreementVM.Loan.Agreement))
                        .ContinueWith(CalculatePaymentAmountCallback, _beginLoanAgreementVM.Loan.Agreement, TaskScheduler.FromCurrentSynchronizationContext());
                    break;
            }
        }

        private LoanPaymentAmountResponse CalculatePaymentAmount(LoanAgreementVM loanAgreementVM)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                if (!loanAgreementVM.HasErrors)
                {

                    ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                    ILoanPaymentAmountService service = scope.Resolve<ILoanPaymentAmountService>();
                    return service.Calculate(
                        settingsFactory.CreateLoanApi(),
                        new LoanPaymentAmountRequest
                        {
                            AnnualInterestRate = loanAgreementVM.InterestPercentage / 100.0M,
                            PaymentFrequency = loanAgreementVM.PaymentFrequency.Value,
                            Term = loanAgreementVM.OriginalTerm.Value,
                            TotalPrincipal = loanAgreementVM.OriginalAmount.Value
                        })
                        .Result;
                }
                else
                {
                    return null;
                }
            }
        }

        private async Task CalculatePaymentAmountCallback(Task<LoanPaymentAmountResponse> calculate, object state)
        {
            try
            {
                LoanPaymentAmountResponse response = await calculate;
                LoanAgreementVM loanAgreementVM = (LoanAgreementVM)state;
                if (response == null || !response.PaymentAmount.HasValue)
                {
                    loanAgreementVM.PaymentAmount = null;
                }
                else
                {
                    loanAgreementVM.PaymentAmount = Math.Round(response.PaymentAmount.Value, 2, MidpointRounding.ToEven);
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }

        public void InitializeInterestRate()
        {
            _beginLoanAgreementVM.BusyVisibility = Visibility.Visible;
            Task.Run(() => _interestRateConfigurationService.Get(_settingsFactory.CreateLoanApi()).Result)
                .ContinueWith(InitializeInterestRateCallback, _beginLoanAgreementVM, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task InitializeInterestRateCallback(Task<InterestRateConfiguration> initialize, object state)
        {
            try
            {
                InterestRateConfiguration interestRateConfiguration = await initialize;
                if (state != null && state is BeginLoanAgreementVM beginLoanAgreementVM)
                {
                    if (beginLoanAgreementVM.Loan.Agreement.InterestPercentage == 0.0M && interestRateConfiguration.TotalRate.HasValue)
                    {
                        beginLoanAgreementVM.Loan.Agreement.InterestPercentage = interestRateConfiguration.TotalRate.Value * 100.0M;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                if (state != null && state is BeginLoanAgreementVM beginLoanAgreementVM)
                {
                    beginLoanAgreementVM.BusyVisibility = Visibility.Collapsed;
                }
            }
        }
    }
}
