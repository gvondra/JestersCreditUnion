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
    public class LookupService : ILookupService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public LookupService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public async Task Delete(ISettings settings, string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Delete)
                .AddPath("Lookup/{code}")
                .AddPathParameter("code", code)
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            IResponse response = await _service.Send(request);
            _restUtil.CheckSuccess(response);
        }

        public Task<Lookup> Get(ISettings settings, string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("Lookup/{code}")
                .AddPathParameter("code", code)
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Lookup>(_service, request);
        }

        public Task<List<string>> GetIndex(ISettings settings)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("LookupIndex")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<string>>(_service, request);
        }

        public Task<Lookup> Save(ISettings settings, string code, Dictionary<string, string> data)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentNullException(nameof(code));
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put, data)
                .AddPath("Lookup/{code}")
                .AddPathParameter("code", code)
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Lookup>(_service, request);
        }
    }
}
