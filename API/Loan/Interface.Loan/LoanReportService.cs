using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public class LoanReportService : ILoanReportService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public LoanReportService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<List<LoanPastDue>> GetPastDue(ISettings settings, short minDays = 60)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("Reporting/Loan/PastDue")
                .AddJwtAuthorizationToken(settings.GetToken);
            return _restUtil.Send<List<LoanPastDue>>(_service, request);
        }
    }
}
