using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public class WorkTaskCycleService : IWorkTaskCycleService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public WorkTaskCycleService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<List<WorkTaskCycleSummaryItem>> GetSummary(ISettings settings, DateTime? minCreateDate = null)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("Reporting/WorkTaskCycle/Summary")
                .AddJwtAuthorizationToken(settings.GetToken);
            if (minCreateDate.HasValue)
            {
                request.AddQueryParameter("minCreateDate", minCreateDate.Value.ToString("O"));
            }
            return _restUtil.Send<List<WorkTaskCycleSummaryItem>>(_service, request);
        }
    }
}
