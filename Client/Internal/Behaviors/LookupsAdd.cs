using JCU.Internal.ViewModel;
using System;
using System.Linq;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class LookupsAdd : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null) 
                throw new ArgumentNullException(nameof(parameter));
            if (!typeof(LookupsVM).IsAssignableFrom(parameter.GetType()))
                throw new ApplicationException($"Parameter must be of type {typeof(LookupsVM).Name}");
            LookupsVM lookupsVM = (LookupsVM)parameter;
            if (!string.IsNullOrEmpty(lookupsVM.NewCode))
            {
                string code = lookupsVM.LookupCodes.FirstOrDefault(c => string.Equals(lookupsVM.NewCode, c, StringComparison.OrdinalIgnoreCase));
                if (code != null)
                {
                    lookupsVM.SelectedLookupCode = code;
                }
                else
                {
                    lookupsVM.LookupCodes.Add(lookupsVM.NewCode);
                    lookupsVM.SelectedLookupCode = lookupsVM.NewCode;
                    lookupsVM.NewCode = string.Empty;
                }
            }
        }
    }
}
