using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Loan.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace JCU.Internal.ViewModel
{
    public class LookupVM : ViewModelBase
    {
        private readonly LookupsVM _lookupVM;
        private readonly Lookup _lookup;
        private readonly ObservableCollection<Item> _items = new ObservableCollection<Item>();
        private LookupValueAdd _valueAdd;
        private LookupSave _save;
        private LookupDelete _delete;

        private LookupVM(Lookup lookup, LookupsVM lookupVM)
        {
            _lookup = lookup;
            _lookupVM = lookupVM;
        }

        private LookupVM(Lookup lookup)
            : this(lookup, null)
        {}

        public LookupsVM LookupsVM => _lookupVM;

        public string Code => _lookup.Code;

        public ObservableCollection<Item> Items => _items;

        public LookupDelete Delete
        {
            get => _delete;
            set
            {
                if (_delete != value)
                {
                    _delete = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public LookupSave Save
        {
            get => _save;
            set
            {
                if (_save != value)
                {
                    _save = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public LookupValueAdd ValueAdd
        {
            get => _valueAdd;
            set
            {
                if (_valueAdd != value)
                {
                    _valueAdd = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private Dictionary<string, string> Data
        {
            get
            {
                if (_lookup.Data == null)
                    _lookup.Data = new Dictionary<string, string>();
                return _lookup.Data;
            }
        }

        public static LookupVM Create(Lookup lookup, LookupsVM lookupsVM)
        {
            LookupVM vm = new LookupVM(lookup, lookupsVM);
            vm.ValueAdd = new LookupValueAdd();
            vm.Save = new LookupSave();
            vm.Delete = new LookupDelete();
            foreach (KeyValuePair<string, string> pair in vm.Data)
            {
                vm.Items.Add(new Item(pair.Key, pair.Value));
            }
            return vm;
        }

        public static LookupVM Create(Lookup lookup)
        {
            LookupVM vm = new LookupVM(lookup);
            foreach (KeyValuePair<string, string> pair in vm.Data)
            {
                vm.Items.Add(new Item(pair.Key, pair.Value));
            }
            return vm;
        }

        public class Item
        {
            public Item(string key, string value)
            {
                Key = key;
                Value = value;
            }

            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
