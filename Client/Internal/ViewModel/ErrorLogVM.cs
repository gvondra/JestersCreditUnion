using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class ErrorLogVM : ViewModelBase
    {
        private readonly ObservableCollection<ExceptionLogItemVM> _items = new ObservableCollection<ExceptionLogItemVM>();

        private Visibility _busyVisibility = Visibility.Collapsed;

        public ObservableCollection<ExceptionLogItemVM> Items => _items;

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

        public static ErrorLogVM Create(ISettingsFactory settingsFactory, IExceptionService exceptionService)
        {            
            ErrorLogVM vm = new ErrorLogVM();
            ExceptionLogLoader loader = new ExceptionLogLoader(vm, settingsFactory, exceptionService);
            vm.AddBehavior(loader);
            loader.Load();
            return vm;
        }
    }
}
