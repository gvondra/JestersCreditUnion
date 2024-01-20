using JestersCreditUnion.Interface.Loan.Models;

namespace JCU.Internal.ViewModel
{
    public class RatingVM : ViewModelBase
    {
        private readonly Rating _innerRating;

        public RatingVM(Rating innerRating)
        {
            _innerRating = innerRating;
        }

        public double? Value => _innerRating.Value;
    }
}
