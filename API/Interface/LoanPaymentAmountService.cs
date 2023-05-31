using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class LoanPaymentAmountService : ILoanPaymentAmountService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public LoanPaymentAmountService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<LoanPaymentAmountResponse> Calculate(ISettings settings, LoanPaymentAmountRequest loanPyamentAmountRequest)
        {
            if (loanPyamentAmountRequest == null)
                throw new ArgumentNullException(nameof(loanPyamentAmountRequest));
            else if (!loanPyamentAmountRequest.AnnualInterestRate.HasValue)
                throw new ArgumentNullException(nameof(LoanPaymentAmountRequest.AnnualInterestRate));
            else if (loanPyamentAmountRequest.AnnualInterestRate.Value <= 0.0M || loanPyamentAmountRequest.AnnualInterestRate.Value >= 1.0M)
                throw new ApplicationException("Interest rate must be between 0.0 and 1.0, exclusive.");
            else if (!loanPyamentAmountRequest.TotalPrincipal.HasValue)
                throw new ArgumentNullException(nameof(LoanPaymentAmountRequest.TotalPrincipal));
            else if (loanPyamentAmountRequest.TotalPrincipal <= 0.0M)
                throw new ApplicationException("Total principal must be greater than zero");
            else if (!loanPyamentAmountRequest.Term.HasValue)
                throw new ArgumentNullException(nameof(LoanPaymentAmountRequest.Term));
            else if (loanPyamentAmountRequest.Term.Value <= 0)
                throw new ApplicationException("Term must be greater than zero");
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post, loanPyamentAmountRequest)
                .AddPath("LoanPaymentAmount")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<LoanPaymentAmountResponse>(_service, request);
        }
    }
}
