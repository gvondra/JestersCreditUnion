using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class WorkTaskConfigurationVM : ViewModelBase
    {
        private readonly WorkTaskConfiguration _innerConfiguration;
        private WorkTaskConfigurationSaver _saver;
        private Visibility _busyVisibility = Visibility.Collapsed;

        private WorkTaskConfigurationVM(WorkTaskConfiguration innerConfiguration)
        {
            _innerConfiguration = innerConfiguration;
        }

        public Visibility BusyVisibility
        {
            get => _busyVisibility;
            set
            {
                if (_busyVisibility != value)
                {
                    _busyVisibility = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public WorkTaskConfigurationSaver WorkTaskConfigurationSaver
        {
            get => _saver;
            set
            {
                _saver = value;
                NotifyPropertyChanged();
            }
        }

        public WorkTaskConfiguration InnerConfiguration => _innerConfiguration;

        public string NewLoanApplicationTaskTypeCode 
        { 
            get => _innerConfiguration.NewLoanApplicationTaskTypeCode ?? string.Empty;
            set
            {
                if (_innerConfiguration.NewLoanApplicationTaskTypeCode == null || _innerConfiguration.NewLoanApplicationTaskTypeCode != value)
                {
                    _innerConfiguration.NewLoanApplicationTaskTypeCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string SendDenialCorrespondenceTaskTypeCode
        {
            get => _innerConfiguration.SendDenialCorrespondenceTaskTypeCode ?? string.Empty;
            set
            {
                if (_innerConfiguration.SendDenialCorrespondenceTaskTypeCode == null || _innerConfiguration.SendDenialCorrespondenceTaskTypeCode != value)
                {
                    _innerConfiguration.SendDenialCorrespondenceTaskTypeCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string DiburseFundsTaskTypeCode
        {
            get => _innerConfiguration.DiburseFundsTaskTypeCode ?? string.Empty;
            set
            {
                if (_innerConfiguration.DiburseFundsTaskTypeCode == null || _innerConfiguration.DiburseFundsTaskTypeCode != value)
                {
                    _innerConfiguration.DiburseFundsTaskTypeCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static WorkTaskConfigurationVM Create(WorkTaskConfiguration workTaskConfiguration, ISettingsFactory settingsFactory, IWorkTaskConfigurationService configService)
        {
            WorkTaskConfigurationVM vm = new WorkTaskConfigurationVM(workTaskConfiguration);
            vm.WorkTaskConfigurationSaver = new WorkTaskConfigurationSaver(settingsFactory, configService);
            return vm;
        }
    }
}
