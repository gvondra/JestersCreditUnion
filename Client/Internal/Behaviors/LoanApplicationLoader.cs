using JCU.Internal.ViewModel;
using JestersCreditUnion.Interface.Loan;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Threading.Tasks;

namespace JCU.Internal.Behaviors
{
    public class LoanApplicationLoader
    {
        private readonly LoanApplicationVM _loanApplicationVM;
        private readonly ISettingsFactory _settingsFactory;
        private readonly ILoanApplicationRatingService _loanApplicationRatingService;

        public LoanApplicationLoader(
            LoanApplicationVM loanApplicationVM,
            ISettingsFactory settingsFactory,
            ILoanApplicationRatingService loanApplicationRatingService)
        {
            _loanApplicationVM = loanApplicationVM;
            _settingsFactory = settingsFactory;
            _loanApplicationRatingService = loanApplicationRatingService;
        }

        public void LoadRating()
        {
            Task.Run(() => _loanApplicationRatingService.Get(_settingsFactory.CreateLoanApi(), _loanApplicationVM.LoanApplicationId.Value))
                .ContinueWith(LoadRatingCallback, null, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadRatingCallback(Task<Rating> loadRating, object state)
        {
            try
            {
                Rating rating = await loadRating;
                _loanApplicationVM.Rating = new RatingVM(rating);
            }
            catch (Exception ex)
            {
                ErrorWindow.Open(ex);
            }
        }
    }
}
