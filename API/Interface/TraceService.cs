using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class TraceService : ITraceService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public TraceService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<List<string>> GetEventCodes(ISettings settings)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("TraceEventCode")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<string>>(_service, request);
        }

        public Task<List<Trace>> Search(ISettings settings, DateTime maxTimestamp, string eventCode)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("Trace")
                .AddQueryParameter("maxTimestamp", maxTimestamp.ToString("o"))
                .AddQueryParameter("eventCode", eventCode)
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<Trace>>(_service, request);
        }
    }
}
