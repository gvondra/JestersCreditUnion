using JCU.Internal.ViewModel;
using System;
using System.Linq;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class WorkGroupMemberAdd : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (typeof(WorkGroupVM).IsAssignableFrom(parameter.GetType()))
            {
                WorkGroupVM workGroupVM = (WorkGroupVM)parameter;
                if (workGroupVM.FoundMember != null)
                {
                    if (!workGroupVM.Members.Any(m => m.UserId.Equals(workGroupVM.FoundMember.UserId)))
                    {
                        workGroupVM.Members.Add(workGroupVM.FoundMember);
                    }
                    workGroupVM.FoundMember = null;
                }
            }
        }
    }
}
