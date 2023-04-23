using JCU.Internal.Behaviors;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCU.Internal.ViewModel
{
    public class LookupVM : ViewModelBase
    {        
        private readonly Lookup _lookup;
        private readonly ObservableCollection<Item> _items = new ObservableCollection<Item>();
        private LookupValueAdd _valueAdd;
        private LookupSave _save;

        private LookupVM(Lookup lookup)
        {
            _lookup = lookup;
        }

        public string Code => _lookup.Code;

        public ObservableCollection<Item> Items => _items;

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

        public static LookupVM Create(Lookup lookup)
        {
            LookupVM vm = new LookupVM(lookup);
            vm.ValueAdd = new LookupValueAdd();
            vm.Save = new LookupSave();
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
