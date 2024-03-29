﻿using Autofac;
using JCU.Internal.ViewModel;
using JCU.Internal.CommonWindow;
using JestersCreditUnion.Interface;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JCU.Internal.NavigationPage.Log
{
    /// <summary>
    /// Interaction logic for ErrorLog.xaml
    /// </summary>
    public partial class ErrorLog : Page
    {
        private ErrorLogItemWindow _errorLogItemWindow;

        public ErrorLog()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            this.Loaded += ErrorLog_Loaded;
        }

        private ErrorLogVM ErrorLogVM { get; set; }

        private void ErrorLog_Loaded(object sender, RoutedEventArgs e)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                IExceptionService exceptionService = scope.Resolve<IExceptionService>();
                ErrorLogVM = ErrorLogVM.Create(settingsFactory, exceptionService);
                DataContext = ErrorLogVM;
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {   
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem != null && dataGrid.SelectedItem is ExceptionLogItemVM logItem)
                {
                    if (_errorLogItemWindow == null)
                    {
                        _errorLogItemWindow = new ErrorLogItemWindow();
                        _errorLogItemWindow.Closed += ErrorLogItemWindow_Closed;
                        _errorLogItemWindow.Show();
                    }
                    _errorLogItemWindow.ExceptionLogItemWindowVM = new ExceptionLogItemWindowVM(logItem);
                    _errorLogItemWindow.Activate();
                }
            }
        }

        private void ErrorLogItemWindow_Closed(object sender, EventArgs e)
        {
            _errorLogItemWindow = null;
        }
    }
}
