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
    public class WorkTaskStatusService : IWorkTaskStatusService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public WorkTaskStatusService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<WorkTaskStatus> Create(ISettings settings, WorkTaskStatus workTaskStatus)
        {
            if (workTaskStatus == null)
                throw new ArgumentNullException(nameof(workTaskStatus));
            if (!workTaskStatus.WorkTaskTypeId.HasValue || workTaskStatus.WorkTaskTypeId.Value.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(workTaskStatus.WorkTaskTypeId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post, workTaskStatus)
                .AddPath("WorkTaskType/{workTaskTypeId}/Status")
                .AddPathParameter("workTaskTypeId", workTaskStatus.WorkTaskTypeId.Value.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkTaskStatus>(_service, request);
        }

        public Task<WorkTaskStatus> Get(ISettings settings, Guid workTaskTypeId, Guid id)
        {
            if (workTaskTypeId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(workTaskTypeId));
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(id));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("WorkTaskType/{workTaskTypeId}/Status/{id}")
                .AddPathParameter("workTaskTypeId", workTaskTypeId.ToString("N"))
                .AddPathParameter("id", id.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkTaskStatus>(_service, request);
        }

        public Task<List<WorkTaskStatus>> GetAll(ISettings settings, Guid workTaskTypeId)
        {
            if (workTaskTypeId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(workTaskTypeId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("WorkTaskType/{workTaskTypeId}/Status")
                .AddPathParameter("workTaskTypeId", workTaskTypeId.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<WorkTaskStatus>>(_service, request);
        }

        public Task<WorkTaskStatus> Update(ISettings settings, WorkTaskStatus workTaskStatus)
        {
            if (workTaskStatus == null)
                throw new ArgumentNullException(nameof(workTaskStatus));
            if (!workTaskStatus.WorkTaskTypeId.HasValue || workTaskStatus.WorkTaskTypeId.Value.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(workTaskStatus.WorkTaskTypeId));
            if (!workTaskStatus.WorkTaskStatusId.HasValue || workTaskStatus.WorkTaskStatusId.Value.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(workTaskStatus.WorkTaskStatusId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put, workTaskStatus)
                .AddPath("WorkTaskType/{workTaskTypeId}/Status/{id}")
                .AddPathParameter("workTaskTypeId", workTaskStatus.WorkTaskTypeId.Value.ToString("N"))
                .AddPathParameter("id", workTaskStatus.WorkTaskStatusId.Value.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<WorkTaskStatus>(_service, request);
        }
    }
}
