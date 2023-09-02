using JestersCreditUnion.Framework;

namespace JestersCreditUnion.Core
{
    public sealed class AmortizationItem : IAmortizationItem
    {
        public short Term { get; internal set; }

        public string Description { get; internal set; }

        public decimal Amount { get; internal set; }

        public decimal Balance { get; internal set; }
    }
}
