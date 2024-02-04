using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public class LoanSummaryService : ILoanSummaryService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public LoanSummaryService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<OpenLoanSummary> GetOpenLoanSummary(ISettings settings)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("Reporting/LoanSummary/Open")
                .AddJwtAuthorizationToken(settings.GetToken);
            return _restUtil.Send<OpenLoanSummary>(_service, request);
        }
    }
}
