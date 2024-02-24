using JCU.Internal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class InterestRateConfigurationValidator
    {
        private readonly InterestRateConfigurationVM _interestRateConfigurationVM;

        public InterestRateConfigurationValidator(InterestRateConfigurationVM interestRateConfigurationVM)
        {
            _interestRateConfigurationVM = interestRateConfigurationVM;
            _interestRateConfigurationVM.PropertyChanged += InterestRateConfigurationVM_PropertyChanged;
        }

        private void InterestRateConfigurationVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_interestRateConfigurationVM[e.PropertyName] != null)
                _interestRateConfigurationVM[e.PropertyName] = null;
            switch (e.PropertyName)
            {

            }
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            decimal total = 0.0M;
            if (_interestRateConfigurationVM.InflationRate.HasValue)
                total += _interestRateConfigurationVM.InflationRate.Value;
            if (_interestRateConfigurationVM.OperationsRate.HasValue)
                total += _interestRateConfigurationVM.OperationsRate.Value;
            if (_interestRateConfigurationVM.LossRate.HasValue)
                total += _interestRateConfigurationVM.LossRate.Value;
            if (_interestRateConfigurationVM.IncentiveRate.HasValue)
                total += _interestRateConfigurationVM.IncentiveRate.Value;
            if (_interestRateConfigurationVM.OtherRate.HasValue)
                total += _interestRateConfigurationVM.OtherRate.Value;
            _interestRateConfigurationVM.TotalRate = total;
        }
    }
}
