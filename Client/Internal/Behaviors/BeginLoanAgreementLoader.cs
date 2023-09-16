using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class BeginLoanAgreementLoader
    {
        private readonly BeginLoanAgreementVM _beginLoanAgreementVM;

        public BeginLoanAgreementLoader(BeginLoanAgreementVM beginLoanAgreementVM)
        {
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
    }
}
