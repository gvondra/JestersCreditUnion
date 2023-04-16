using JestersCreditUnion.Interface.Models;

namespace JCU.Internal.ViewModel
{
    public class WorkTaskVM : ViewModelBase
    {
        private readonly WorkTask _workTask;

        private WorkTaskVM(WorkTask workTask)
        {
            _workTask = workTask;
        }

        public WorkTask InnerWorkTask => _workTask;

        public string Title
        {
            get => _workTask.Title;
            set
            {
                if (_workTask.Title != value)
                {
                    _workTask.Title = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public static WorkTaskVM Create(WorkTask workTask)
        {
            WorkTaskVM vm = new WorkTaskVM(workTask);
            return vm;
        }
    }
}
