using BrassLoon.RestClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class TokenService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public TokenService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<string> Create(ISettings settings)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post)
                .AddPath("Token")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<string>(_service, request);
        }
    }
}
