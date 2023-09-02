using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class TokenService : ITokenService
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

        public Task<string> CreateClientCredential(ISettings settings, ClientCredential clientCredential)
        {
            if (clientCredential == null)
                throw new ArgumentNullException(nameof(clientCredential));
            if (!clientCredential.ClientId.HasValue)
                throw new ArgumentException($"{nameof(clientCredential.ClientId)} is required");
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post, clientCredential)
                .AddPath("Token/ClientCredential")
                ;
            return _restUtil.Send<string>(_service, request);
        }
    }
}
