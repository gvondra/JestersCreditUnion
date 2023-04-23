using JCU.Internal.Behaviors;
using System.Collections.ObjectModel;

namespace JCU.Internal.ViewModel
{
    public class LookupsVM : ViewModelBase
    {
        private readonly ObservableCollection<string> _lookupCodes = new ObservableCollection<string>();
        private LookupsAdd _add;
        private string _newCode;
        private string _selectedLookupCode;
        private LookupVM _selectedLookup;

        private LookupsVM() { }

        public ObservableCollection<string> LookupCodes => _lookupCodes;

        public string SelectedLookupCode
        {
            get => _selectedLookupCode;
            set 
            { 
                if (_selectedLookupCode != value)
                {
                    _selectedLookupCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public LookupVM SelectedLookup
        {
            get => _selectedLookup;
            set
            {
                if (_selectedLookup != value)
                {
                    _selectedLookup = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string NewCode
        {
            get => _newCode;
            set
            {
                if (_newCode != value)
                {
                    _newCode = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public LookupsAdd Add
        {
            get => _add;
            set
            {
                if (_add != value)
                {
                    _add = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static LookupsVM Create()
        {
            LookupsVM vm = new LookupsVM();
            vm.Add = new LookupsAdd();
            LookupsLoader loader = new LookupsLoader(vm);
            vm.AddBehavior(loader);
            loader.Load();
            return vm;
        }
    }
}
