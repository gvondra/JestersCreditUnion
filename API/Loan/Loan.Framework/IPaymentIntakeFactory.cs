using JestersCreditUnion.Loan.Framework.Enumerations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Framework
{
    public interface IPaymentIntakeFactory
    {
        ILoanFactory LoanFactory { get; }
        IPaymentIntake Create(ILoan loan);
        Task<IPaymentIntake> Get(ISettings settings, Guid id);
        Task<IEnumerable<IPaymentIntake>> GetByStatues(ISettings settings, IEnumerable<PaymentIntakeStatus> statuses);
    }
}
