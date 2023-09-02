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
    /// Interaction logic for User.xaml
    /// </summary>
    public partial class User : Page
    {
        public User()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            this.Loaded += User_Loaded;
        }

        private FindUserVM FindUserVM { get; set; }

        private void User_Loaded(object sender, RoutedEventArgs e)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                IUserService userService = scope.Resolve<IUserService>();
                IRoleService roleService = scope.Resolve<IRoleService>();
                FindUserVM = FindUserVM.Create(settingsFactory, roleService, userService);
                DataContext = FindUserVM;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FindUserVM.Save != null)
                    FindUserVM.Save();
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FindUserVM.FindUser != null)
                    FindUserVM.FindUser();
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }
    }
}
