using JestersCreditUnion.Interface.Loan.Models;

namespace JCU.Internal.ViewModel
{
    public class AmortizationItemVM : ViewModelBase
    {
        private readonly AmortizationItem _amortizationItem;

        private AmortizationItemVM(AmortizationItem amortizationItem)
        {
            _amortizationItem = amortizationItem;
        }

        public short Term => _amortizationItem.Term ?? 0;
        public string Description => _amortizationItem.Description ?? string.Empty;
        public decimal Amount => _amortizationItem.Amount ?? 0.0M;
        public decimal Balance => _amortizationItem.Balance ?? 0.0M;

        public static AmortizationItemVM Create(AmortizationItem amortizationItem)
        {
            AmortizationItemVM vm = new AmortizationItemVM(amortizationItem);
            return vm;
        }
    }
}
