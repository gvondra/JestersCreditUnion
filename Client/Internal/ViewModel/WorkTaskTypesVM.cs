using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.ViewModel
{
    public class WorkTaskTypesVM : ViewModelBase
    {
        private readonly ObservableCollection<WorkTaskTypeVM> _items = new ObservableCollection<WorkTaskTypeVM>();

        public ObservableCollection<WorkTaskTypeVM> Items => _items;

        public static WorkTaskTypesVM Create(ISettingsFactory settingsFactory, 
            IWorkTaskTypeService workTaskTypeService,
            IWorkTaskStatusService workTaskStatusService)
        {
            WorkTaskTypesVM vm = new WorkTaskTypesVM();
            WorkTaskTypesLoader loader = new WorkTaskTypesLoader(vm, settingsFactory, workTaskTypeService, workTaskStatusService);
            vm.AddBehavior(loader);
            loader.Load();
            return vm;
        }
    }
}
