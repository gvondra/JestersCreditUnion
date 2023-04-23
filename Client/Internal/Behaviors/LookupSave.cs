using Autofac;
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
    public class LookupSave : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (!typeof(LookupVM).IsAssignableFrom(parameter.GetType()))
                throw new ApplicationException($"Parameter must be of type {typeof(LookupVM).Name}");
            _canExecute = false;
            CanExecuteChanged.Invoke(this, new EventArgs());
            LookupVM lookupVM = (LookupVM)parameter;
            Task.Run(() =>
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                    ILookupService lookupService = scope.Resolve<ILookupService>();
                    Dictionary<string ,string> data = lookupVM.Items.ToDictionary(i => i.Key, i => i.Value);
                    lookupService.Save(settingsFactory.CreateApi(), lookupVM.Code, data).Wait();
                }
            })
                .ContinueWith(SaveCallback, parameter, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task SaveCallback(Task save, object parameter)
        {
            try
            {
                await save;
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
