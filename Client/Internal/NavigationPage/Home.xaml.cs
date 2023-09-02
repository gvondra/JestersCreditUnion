﻿using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JCU.Internal.NavigationPage
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            this.Loaded += Home_Loaded;
            AccessToken.Get.PropertyChanged += AccessToken_PropertyChanged;
        }

        private HomeVM HomeVM { get; set; }

        private void AccessToken_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AccessToken.Token):
                    UpdateAccessBasedVisibility();
                    LoadDocument();
                    break;
            }
        }

        private void UpdateAccessBasedVisibility()
        {
            if (HomeVM != null)
            {
                if (AccessToken.Get.UserHasClaimWorkTaskAccess())
                {
                    HomeVM.WorkTasksHomeVisibility = Visibility.Visible;
                    if (HomeVM.LoadWorkTasks != null)
                        HomeVM.LoadWorkTasks();
                }
                else
                {
                    HomeVM.WorkTasksHomeVisibility = Visibility.Collapsed;
                }
            }
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    HomeVM = HomeVM.Create(scope.Resolve<ISettingsFactory>(),
                        scope.Resolve<IWorkGroupService>(),
                        scope.Resolve<IUserService>(),
                        scope.Resolve<IWorkTaskService>());
                    DataContext = HomeVM;
                    UpdateAccessBasedVisibility();
                    LoadDocument();
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }

        private void LoadDocument()
        {
            HomeVM.Document = new FlowDocument();
            Paragraph paragraph = new Paragraph(new Run("Welcome"));
            HomeVM.Document.Blocks.Add(paragraph);
            if (AccessToken.Get.UserHasReadLoanAccess())
            {
                paragraph = new Paragraph();
                HomeVM.Document.Blocks.Add(paragraph);
                Hyperlink hyperlink = new Hyperlink(new Run("Loan Search"));
                hyperlink.Click += SearchHyperlink_Click;
                paragraph.Inlines.Add(hyperlink);
            }
        }

        private void SearchHyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService navigationService = NavigationService.GetNavigationService(this);
                navigationService.Navigate(new Uri("NavigationPage/LoanSearch.xaml", UriKind.Relative));
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }
    }
}
