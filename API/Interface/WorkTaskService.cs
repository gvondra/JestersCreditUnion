using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class WorkTaskService : IWorkTaskService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public WorkTaskService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<ClaimWorkTaskResponse> Claim(ISettings settings, Guid id, string assignToUserId, DateTime? assignedDate = null)
        {
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(id));
            if (assignToUserId == null)
                assignToUserId = string.Empty;
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put)
                .AddPath("WorkTask/{id}/AssignTo")
                .AddPathParameter("id", id.ToString("N"))
                .AddQueryParameter("assignToUserId", assignToUserId)
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            if (assignedDate.HasValue)
            {
                request.AddQueryParameter("assignedDate", assignedDate.Value.Date.ToString("O"));
            }
            return _restUtil.Send<ClaimWorkTaskResponse>(_service, request);
        }

        public Task<List<WorkTask>> GetByContext(ISettings settings, short referenceType, string referenceValue, bool? includeClosed = null)
        {
            if (string.IsNullOrEmpty(referenceValue))
                throw new ArgumentNullException(nameof(referenceValue));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("WorkTask")
                .AddQueryParameter("referenceType", referenceType.ToString(CultureInfo.InvariantCulture))
                .AddQueryParameter("referenceValue", referenceValue)
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            if (includeClosed.HasValue)
                request.AddQueryParameter("includeClosed", includeClosed.Value.ToString());
            return _restUtil.Send<List<Models.WorkTask>>(_service, request);
        }

        public Task<List<WorkTask>> GetByWorkGroupId(ISettings settings, Guid workGroupId, bool? includeClosed = null)
        {
            if (workGroupId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(workGroupId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("WorkGroup/{id}/WorkTask")
                .AddPathParameter("id", workGroupId.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            if (includeClosed != null)
                request.AddQueryParameter("includeClosed", includeClosed.Value.ToString());
            return _restUtil.Send<List<Models.WorkTask>>(_service, request);
        }

        public Task<List<WorkTask>> Patch(ISettings settings, IEnumerable<Dictionary<string, object>> patchData)
        {
            if (patchData == null)
                throw new ArgumentNullException(nameof(patchData));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), new HttpMethod("PATCH"), patchData.ToList())
                .AddPath("WorkTask")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<Models.WorkTask>>(_service, request);
        }

        public Task<WorkTask> Create(ISettings settings, WorkTask workTask)
        {
            if (workTask == null)
                throw new ArgumentNullException(nameof(workTask));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post, workTask)
                .AddPath("WorkTask")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.WorkTask>(_service, request);
        }

        public Task<WorkTask> Update(ISettings settings, WorkTask workTask)
        {
            if (workTask == null)
                throw new ArgumentNullException(nameof(workTask));
            if (!workTask.WorkTaskId.HasValue || workTask.WorkTaskId.Value.Equals(Guid.Empty))
                throw new ArgumentException($"{nameof(WorkTask.WorkTaskId)} is null");
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put, workTask)
                .AddPath("WorkTask/{id}")
                .AddPathParameter("id", workTask.WorkTaskId.Value.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.WorkTask>(_service, request);
        }
    }
}
