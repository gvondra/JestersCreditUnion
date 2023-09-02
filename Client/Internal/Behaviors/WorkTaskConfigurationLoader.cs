using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class WorkTaskConfigurationLoader
    {
        private readonly ISettingsFactory _settingsFactory;
        private readonly IWorkTaskConfigurationService _workTaskConfigurationService;

        public WorkTaskConfigurationLoader(ISettingsFactory settingsFactory, IWorkTaskConfigurationService workTaskConfigurationService)
        {
            _settingsFactory = settingsFactory;
            _workTaskConfigurationService = workTaskConfigurationService;
        }

        public void Load(Action<WorkTaskConfigurationVM> callback)
        {   
            Task.Run(() =>
            {
                ISettings settings = _settingsFactory.CreateApi();
                return _workTaskConfigurationService.Get(settings).Result;
            })
                .ContinueWith(LoadCallback, callback, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadCallback(Task<WorkTaskConfiguration> load, object state)
        {
            try
            {
                WorkTaskConfiguration config = await load;
                Action<WorkTaskConfigurationVM> callback = (Action<WorkTaskConfigurationVM>)state;
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    callback.Invoke(WorkTaskConfigurationVM.Create(config,
                        scope.Resolve<ISettingsFactory>(),
                        scope.Resolve<IWorkTaskConfigurationService>()));
                }
            }
            catch(System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
