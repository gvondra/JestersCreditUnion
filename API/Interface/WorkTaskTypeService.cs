using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class WorkTaskTypeService : IWorkTaskTypeService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public WorkTaskTypeService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<WorkTaskType> Create(ISettings settings, WorkTaskType workTaskType)
        {
            if (workTaskType == null)
                throw new ArgumentNullException(nameof(workTaskType));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post, workTaskType)
                .AddPath("WorkTaskType")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkTaskType>(_service, request);
        }

        public Task<WorkTaskType> Get(ISettings settings, Guid id)
        {
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(id));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("WorkTaskType")
                .AddPath(id.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkTaskType>(_service, request);
        }

        public Task<List<WorkTaskType>> GetAll(ISettings settings)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("WorkTaskType")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<WorkTaskType>>(_service, request);
        }

        public Task<WorkTaskType> GetByCode(ISettings settings, string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("WorkTaskType")
                .AddQueryParameter("code", code)
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkTaskType>(_service, request);
        }

        public Task<List<WorkTaskType>> GetByWorkGroupId(ISettings settings, Guid workGroupId)
        {
            if (workGroupId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(workGroupId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("WorkGroup/{workGroupId}/WorkTaskType")
                .AddPathParameter("workGroupId", workGroupId.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<WorkTaskType>>(_service, request);
        }

        public Task<WorkTaskType> Update(ISettings settings, WorkTaskType workTaskType)
        {
            if (workTaskType == null)
                throw new ArgumentNullException(nameof(workTaskType));
            if (!workTaskType.WorkTaskTypeId.HasValue || workTaskType.WorkTaskTypeId.Value.Equals(Guid.Empty))
                throw new ArgumentException($"{nameof(workTaskType.WorkTaskTypeId)} is null");
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put, workTaskType)
                .AddPath("WorkTaskType")
                .AddPath(workTaskType.WorkTaskTypeId.Value.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkTaskType>(_service, request);
        }
    }
}
