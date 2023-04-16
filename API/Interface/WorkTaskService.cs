using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
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
    }
}
