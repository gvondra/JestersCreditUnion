using Autofac;
using JCU.Internal.Behaviors;
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
    /// Interaction logic for Roles.xaml
    /// </summary>
    public partial class Roles : Page
    {
        public Roles()
        {
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
            InitializeComponent();
            DataContext = null;
            this.Loaded += Roles_Loaded;
        }

        private RolesVM RolesVM { get; set; }

        private void Roles_Loaded(object sender, RoutedEventArgs e)
        {
            using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
            {
                ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                IRoleService roleService = scope.Resolve<IRoleService>();
                RolesVM = RolesVM.Create(settingsFactory, roleService);
                DataContext = RolesVM;
            }
        }

        private void AddHyperlink_Click(object sender, RoutedEventArgs e)
        {
            RoleVM roleVM = new RoleVM();
            RolesVM.Roles.Add(roleVM);
            RolesVM.SelectedRole = roleVM;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((e.AddedItems?.Count ?? 0) > 0)
            {
                RoleVM roleVM = (RoleVM)e.AddedItems[0];
                RolesVM.SelectedRole = roleVM;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    RoleSaver roleSaver = scope.Resolve<RoleSaver>(new NamedParameter("rolesVM", RolesVM));
                    roleSaver.Save(RolesVM.SelectedRole);
                }
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex, Window.GetWindow(this));
            }
        }
    }
}
