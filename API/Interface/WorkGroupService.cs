using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class WorkGroupService : IWorkGroupService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public WorkGroupService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public async Task AddWorkTaskTypeLink(ISettings settings, Guid workGroupId, Guid workTaskTypeId)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post)
                .AddPath("WorkGroup")
                .AddPath(workGroupId.ToString("N"))
                .AddQueryParameter("workTaskTypeId", workTaskTypeId.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            IResponse response = await _service.Send(request);
            _restUtil.CheckSuccess(response);
        }

        public Task<WorkGroup> Create(ISettings settings, WorkGroup workGroup)
        {
            if (workGroup == null)
                throw new ArgumentNullException(nameof(workGroup));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post, workGroup)
                .AddPath("WorkGroup")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkGroup>(_service, request);
        }

        public async Task DeleteWorkTaskTypeLink(ISettings settings, Guid workGroupId, Guid workTaskTypeId)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Delete)
                .AddPath("WorkGroup")
                .AddPath(workGroupId.ToString("N"))
                .AddQueryParameter("workTaskTypeId", workTaskTypeId.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            IResponse response = await _service.Send(request);
            _restUtil.CheckSuccess(response);
        }

        public Task<WorkGroup> Get(ISettings settings, Guid id)
        {
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(id));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("WorkGroup")
                .AddPath(id.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkGroup>(_service, request);
        }

        public Task<List<WorkGroup>> GetAll(ISettings settings)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("WorkGroup")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<WorkGroup>>(_service, request);
        }

        public Task<WorkGroup> Update(ISettings settings, WorkGroup workGroup)
        {
            if (workGroup == null)
                throw new ArgumentNullException(nameof(workGroup));
            if (!workGroup.WorkGroupId.HasValue || workGroup.WorkGroupId.Value.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(workGroup.WorkGroupId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put, workGroup)
                .AddPath("WorkGroup")
                .AddPath(workGroup.WorkGroupId.Value.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkGroup>(_service, request);
        }
    }
}
