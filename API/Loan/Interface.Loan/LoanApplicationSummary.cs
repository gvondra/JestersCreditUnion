using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public class LoanApplicationSummary : ILoanApplicationSummary
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public LoanApplicationSummary(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<List<LoanApplicationSummaryItem>> Get(ISettings settings, DateTime? minAppliationDate = null)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("Reporting/LoanApplicationSummary")
                .AddJwtAuthorizationToken(settings.GetToken);
            if (minAppliationDate.HasValue)
            {
                request.AddQueryParameter("minApplicationDate", minAppliationDate.Value.ToString("O"));
            }
            return _restUtil.Send<List<LoanApplicationSummaryItem>>(_service, request);
        }
    }
}
