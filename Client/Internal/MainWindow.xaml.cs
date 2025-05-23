﻿using JCU.Internal.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace JCU.Internal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {            
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private MainWindowVM MainWindowVM { get; set; }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindowVM = new MainWindowVM();
            DataContext = MainWindowVM;
        }

        private void CloseCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        private void GoToPageCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NavigationService navigationService = navigationFrame.NavigationService;
            JournalEntry journalEntry = navigationService.RemoveBackEntry();
            while (journalEntry != null)
                journalEntry = navigationService.RemoveBackEntry();
            //NavigationService navigationService = NavigationService.GetNavigationService(navigationFrame);
            navigationService.Navigate(new Uri((string)e.Parameter, UriKind.Relative));
        }

        private void GoogleLoginMenuItem_Click(object sender, RoutedEventArgs e)
        {
            GoogleLogin.ShowLoginDialog(checkAccessToken: false, owner: this);
        }

        public void AfterTokenRefresh()
        {
            MainWindowVM.ShowUserRole = BoolToVisibility(AccessToken.Get.UserHasUserAdminRoleAccess());
            MainWindowVM.ShowLogs = BoolToVisibility(AccessToken.Get.UserHasLogReadAccess());
            MainWindowVM.ShowWorkTaskTypeEdit = BoolToVisibility(AccessToken.Get.UserHasTaskTypeEditAccess());
            MainWindowVM.ShowLookups = BoolToVisibility(AccessToken.Get.UserHasLookupEditAccess());
            MainWindowVM.ShowInterestRateConfiguration = BoolToVisibility(AccessToken.Get.UserHasInterestRateConfigureAccess());
        }

        private Visibility BoolToVisibility(bool value) => value ? Visibility.Visible : Visibility.Collapsed;

    }
}
