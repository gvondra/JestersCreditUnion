using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class WorkTaskConfigurationService : IWorkTaskConfigurationService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public WorkTaskConfigurationService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<WorkTaskConfiguration> Get(ISettings settings)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("WorkTaskConfiguration")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkTaskConfiguration>(_service, request);
        }

        public Task<WorkTaskConfiguration> Save(ISettings settings, WorkTaskConfiguration workTaskConfiguration)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put, workTaskConfiguration)
                .AddPath("WorkTaskConfiguration")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkTaskConfiguration>(_service, request);
        }
    }
}
