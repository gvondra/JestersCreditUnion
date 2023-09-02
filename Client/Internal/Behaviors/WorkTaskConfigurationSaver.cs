using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class WorkTaskConfigurationSaver : ICommand
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkTaskConfigurationService _configService;
        private bool _canExecute = true;

        public WorkTaskConfigurationSaver(ISettingsFactory settingsFactory, IWorkTaskConfigurationService configService)
        {
            _settingsFactory = settingsFactory;
            _configService = configService;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (!typeof(WorkTaskConfigurationVM).IsAssignableFrom(parameter.GetType()))
                throw new ApplicationException($"Requires {nameof(parameter)} of type {typeof(WorkTaskConfigurationVM).FullName}");
            _canExecute = false;
            CanExecuteChanged.Invoke(this, new EventArgs());
            WorkTaskConfigurationVM vm = (WorkTaskConfigurationVM)parameter;
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                _configService.Save(settings, vm.InnerConfiguration).Wait();
            })
                .ContinueWith(SaveCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task SaveCallback(Task save, object state)
        {
            try
            {
                await save;
            }
            catch(System.Exception ex)
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
