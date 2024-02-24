using System.Windows;

namespace JCU.Internal.ViewModel
{
    public class MainWindowVM : ViewModelBase
    {
        private Visibility _showUserAdmin = Visibility.Collapsed;
        private Visibility _showUserRole = Visibility.Collapsed;
        private Visibility _showLogs = Visibility.Collapsed;
        private Visibility _showWorkTaskTypeEdit = Visibility.Collapsed;
        private Visibility _showLookups = Visibility.Collapsed;
        private Visibility _showInterestRateConfiguration = Visibility.Collapsed;

        public Visibility ShowInterestRateConfiguration
        {
            get => _showInterestRateConfiguration;
            set
            {
                if (_showInterestRateConfiguration != value)
                {
                    _showInterestRateConfiguration = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility ShowLookups
        {
            get => _showLookups;
            set
            {
                if (_showLookups != value)
                {
                    _showLookups = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility ShowWorkTaskTypeEdit
        {
            get => _showWorkTaskTypeEdit;
            set
            {
                if (_showWorkTaskTypeEdit != value)
                {
                    _showWorkTaskTypeEdit = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility ShowUserAdmin
        {
            get => _showUserAdmin;
            set
            {
                if (_showUserAdmin != value)
                {
                    _showUserAdmin = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Visibility ShowUserRole
        {
            get => _showUserRole;
            set
            {
                if (_showUserRole != value)
                {
                    _showUserRole = value;
                    NotifyPropertyChanged();
                    SetShowUserAdmin();
                }
            }
        }

        public Visibility ShowLogs
        {
            get => _showLogs;
            set
            {
                if (_showLogs != value)
                {
                    _showLogs = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private void SetShowUserAdmin()
            => ShowUserAdmin = ShowUserRole;

    }
}
