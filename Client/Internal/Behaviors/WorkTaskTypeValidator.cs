using JCU.Internal.ViewModel;

namespace JCU.Internal.Behaviors
{
    public class WorkTaskTypeValidator
    {
        private readonly WorkTaskTypeVM _workTaskTypeVM;

        public WorkTaskTypeValidator(WorkTaskTypeVM workTaskTypeVM)
        {
            _workTaskTypeVM = workTaskTypeVM;
            _workTaskTypeVM.PropertyChanged += WorkTaskTypeVM_PropertyChanged;
        }

        private void WorkTaskTypeVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(WorkTaskTypeVM.Code):
                    RequiredTextField(e.PropertyName, _workTaskTypeVM.Code);
                    break;
                case nameof(WorkTaskTypeVM.Title):
                    RequiredTextField(e.PropertyName, _workTaskTypeVM.Title);
                    break;
            }
        }

        private void RequiredTextField(string propertyName, string value)
        {
            string message = null;
            if (string.IsNullOrEmpty(value))
                message = "Is required";
            _workTaskTypeVM[propertyName] = message;
        }
    }
}
