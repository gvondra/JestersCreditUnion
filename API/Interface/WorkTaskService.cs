﻿using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
