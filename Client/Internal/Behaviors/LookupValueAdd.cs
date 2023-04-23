using JCU.Internal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class LookupValueAdd : ICommand
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
            lookupVM.Items.Add(new LookupVM.Item("new", "item"));
        }
    }
}
