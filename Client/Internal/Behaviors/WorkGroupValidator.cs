using JCU.Internal.ViewModel;

namespace JCU.Internal.Behaviors
{
    public class WorkGroupValidator
    {
        private readonly WorkGroupVM _workGroupVM;

        public WorkGroupValidator(WorkGroupVM workGroupVM)
        {
            _workGroupVM = workGroupVM;
            _workGroupVM.PropertyChanged += WorkGroupVM_PropertyChanged;
        }

        private void WorkGroupVM_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(WorkGroupVM.Title):
                    RequiredTextField(e.PropertyName, _workGroupVM.Title);
                    break;
            }
        }

        private void RequiredTextField(string propertyName, string value)
        {
            string message = null;
            if (string.IsNullOrEmpty(value))
                message = "Is required";
            _workGroupVM[propertyName] = message;
        }
    }
}
