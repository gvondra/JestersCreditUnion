using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class LookupDelete : ICommand
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
            LookupVM lookupVM = (LookupVM)parameter;
            MessageBoxResult messageBoxResult = MessageBox.Show($"Are you sure you want to delete {lookupVM.Code}", "Confirm delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                _canExecute = false;
                CanExecuteChanged.Invoke(this, new EventArgs());
                Task.Run(() =>
                {
                    using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                    {
                        ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                        ILookupService lookupService = scope.Resolve<ILookupService>();
                        lookupService.Delete(settingsFactory.CreateLoanApi(), lookupVM.Code).Wait();
                    }
                })
                    .ContinueWith(DeleteCallback, parameter, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        private async Task DeleteCallback(Task delete, object parameter)
        {
            try
            {
                await delete;
                LookupVM lookupVM = (LookupVM)parameter;
                lookupVM.LookupsVM.SelectedLookup = null;
                lookupVM.LookupsVM.SelectedLookupCode = null;
                for (int i = lookupVM.LookupsVM.LookupCodes.Count - 1; i >= 0; i -= 1)
                {
                    if (string.Equals(lookupVM.LookupsVM.LookupCodes[i], lookupVM.Code, StringComparison.OrdinalIgnoreCase))
                    {
                        lookupVM.LookupsVM.LookupCodes.RemoveAt(i);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
