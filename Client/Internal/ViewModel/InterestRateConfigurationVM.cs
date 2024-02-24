using JestersCreditUnion.Interface.Loan.Models;
using System.Windows.Input;

namespace JCU.Internal.ViewModel
{
    public class InterestRateConfigurationVM : ViewModelBase
    {
        private decimal? _inflationRate;
        private decimal? _operationsRate;
        private decimal? _lossRate;
        private decimal? _incentiveRate;
        private decimal? _otherRate;
        private decimal? _totalRate;
        private decimal? _minimumRate;
        private decimal? _maximumRate;
        private string _otherRateDescription = string.Empty;
        private bool _isLoading = false;
        private ICommand _load;
        private ICommand _save;

        public ICommand Load
        {
            get => _load;
            set
            {
                if (_load != value)
                {
                    _load = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public ICommand Save
        {
            get => _save;
            set
            {
                if (_save != value)
                {
                    _save = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            { 
                if (_isLoading != value)
                {
                    _isLoading = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public decimal? InflationRate
        {
            get => _inflationRate;
            set => SetValue<decimal>(value, ref _inflationRate);
        }

        public decimal? OperationsRate
        {
            get => _operationsRate;
            set => SetValue<decimal>(value, ref _operationsRate);
        }

        public decimal? LossRate
        {
            get => _lossRate;
            set => SetValue<decimal>(value, ref _lossRate);
        }

        public decimal? IncentiveRate
        {
            get => _incentiveRate;
            set => SetValue<decimal>(value, ref _incentiveRate);
        }

        public decimal? OtherRate
        {
            get => _otherRate;
            set => SetValue<decimal>(value, ref _otherRate);
        }

        public decimal? TotalRate
        {
            get => _totalRate;
            set => SetValue<decimal>(value, ref _totalRate);
        }

        public decimal? MinimumRate
        {
            get => _minimumRate;
            set => SetValue<decimal>(value, ref _minimumRate);
        }

        public decimal? MaximumRate
        {
            get => _maximumRate;
            set => SetValue<decimal>(value, ref _maximumRate);
        }

        public string OtherRateDescription
        {
            get => _otherRateDescription ?? string.Empty;
            set
            {
                if (_otherRateDescription != (value ?? string.Empty))
                {
                    _otherRateDescription = (value ?? string.Empty);
                    NotifyPropertyChanged();
                }
            }
        }

        public void LoadConfiguration(InterestRateConfiguration interestRateConfiguration)
        {
            InflationRate = interestRateConfiguration.InflationRate;
            OperationsRate = interestRateConfiguration.OperationsRate;
            LossRate = interestRateConfiguration.LossRate;
            IncentiveRate = interestRateConfiguration.IncentiveRate;
            OtherRate = interestRateConfiguration.OtherRate;
            TotalRate = interestRateConfiguration.TotalRate;
            MinimumRate = interestRateConfiguration.MinimumRate;
            MaximumRate = interestRateConfiguration.MaximumRate;
            OtherRateDescription = interestRateConfiguration.OtherRateDescription;
        }
    }
}
