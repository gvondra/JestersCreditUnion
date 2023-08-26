using JestersCreditUnion.Framework;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Core
{
    public class LoanPaymentProcessor : ILoanPaymentProcessor
    {
        public Task ProcessPayments() => throw new NotImplementedException();
    }
}
