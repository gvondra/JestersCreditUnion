using JCU.Internal.ViewModel;
using System;
using System.Windows.Input;

namespace JCU.Internal.Behaviors
{
    public class WorkGroupMemberRemove : ICommand
    {
        private bool _canExecute = true;

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => _canExecute;

        public void Execute(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (typeof(WorkGroupMemberVM).IsAssignableFrom(parameter.GetType()))
            {
                WorkGroupMemberVM workGroupMemberVM = (WorkGroupMemberVM)parameter;
                for (int i = workGroupMemberVM.WorkGroupVM.Members.Count - 1; i >= 0; i -= 1)
                {
                    if (workGroupMemberVM.WorkGroupVM.Members[i].UserId.Equals(workGroupMemberVM.UserId))
                    {
                        workGroupMemberVM.WorkGroupVM.Members.RemoveAt(i);
                    }
                }
            }
        }
    }
}
