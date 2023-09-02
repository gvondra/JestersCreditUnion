using Autofac;
using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace JCU.Internal.Behaviors
{
    public class IdentificationCardLoader
    {
        private readonly LoanApplicationVM _loanApplicationVM;

        public IdentificationCardLoader(LoanApplicationVM loanApplicationVM)
        {
            _loanApplicationVM = loanApplicationVM;
        }

        public void LoadBorrowerIdentificationCard()
        {
            _loanApplicationVM.BusyLoadingBorrowerCardVisibility = Visibility.Visible;
            Task.Run(() =>
            {
                using (ILifetimeScope scope = DependencyInjection.ContainerFactory.Container.BeginLifetimeScope())
                {
                    ISettingsFactory settingsFactory = scope.Resolve<ISettingsFactory>();
                    IIdentificationCardService identificationCardService = scope.Resolve<IIdentificationCardService>();
                    return identificationCardService.GetBorrowerIdentificationCard(settingsFactory.CreateApi(), _loanApplicationVM.LoanApplicationId.Value).Result;                    
                }
            }).ContinueWith(LoadBorrowerIdentificationCardCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadBorrowerIdentificationCardCallback(Task<Stream> load, object state)
        {
            try
            {
                Stream stream = await load;
                BitmapImage image = null;
                if (stream != null && stream.Length > 0)
                {
                    image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = stream;
                    image.EndInit();
                }
                _loanApplicationVM.BorrowerIdentificationImage = image;
            }
            catch (System.Exception ex)
            {
                ErrorWindow.Open(ex);
            }
            finally
            {
                _loanApplicationVM.BusyLoadingBorrowerCardVisibility = Visibility.Collapsed;
            }
        }
    }
}
