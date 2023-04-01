using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.ViewModel
{
    public class WorkTaskConfigurationVM : ViewModelBase
    {
        private readonly WorkTaskConfiguration _innerConfiguration;
        private WorkTaskConfigurationSaver _saver;

        private WorkTaskConfigurationVM(WorkTaskConfiguration innerConfiguration)
        {
            _innerConfiguration = innerConfiguration;
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

        public static WorkTaskConfigurationVM Create(WorkTaskConfiguration workTaskConfiguration, ISettingsFactory settingsFactory, IWorkTaskConfigurationService configService)
        {
            WorkTaskConfigurationVM vm = new WorkTaskConfigurationVM(workTaskConfiguration);
            vm.WorkTaskConfigurationSaver = new WorkTaskConfigurationSaver(settingsFactory, configService);
            return vm;
        }
    }
}
