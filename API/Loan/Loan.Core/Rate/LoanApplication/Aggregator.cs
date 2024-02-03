using JestersCreditUnion.Loan.Framework;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace JestersCreditUnion.Loan.Core.Rate.LoanApplication
{
    internal sealed class Aggregator : IComponent
    {
        private readonly List<IComponent> _components = new List<IComponent>();

        public int Points => _components.Sum(c => c.Points);

        public IEnumerable<IRatingLog> Evaluate(ILoanApplication loanApplication, int totalPoints)
        {
            ImmutableList<IRatingLog> result = [];
            foreach (IComponent component in _components)
            {
                result = result.AddRange(
                    component.Evaluate(loanApplication, totalPoints));
            }
            return result;
        }

        internal void Add(IComponent component) => _components.Add(component);
    }
}
