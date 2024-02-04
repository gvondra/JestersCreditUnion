using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models = JestersCreditUnion.Interface.Models;

namespace JCU.Internal.Behaviors
{
    public class ExceptionLogLoader
    {
        private readonly ErrorLogVM _errorLogVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly IExceptionService _exceptionService;

        public ExceptionLogLoader(ErrorLogVM errorLogVM, ISettingsFactory settingsFactory, IExceptionService exceptionService)
        {
            _errorLogVM = errorLogVM;
            _settingsFactory = settingsFactory;
            _exceptionService = exceptionService;
        }

        public void Load()
        {
            _errorLogVM.BusyVisibility = System.Windows.Visibility.Visible;
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                return _exceptionService.Search(settings, DateTime.UtcNow);
            })
                .ContinueWith(LoadCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadCallback(Task<List<Models.Exception>> load, object state)
        {
            try
            {
                _errorLogVM.Items.Clear();
                foreach (Models.Exception exception in await load)
                {
                    _errorLogVM.Items.Add(new ExceptionLogItemVM(exception));
                }
            }
            catch (System.Exception ex) 
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _errorLogVM.BusyVisibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
