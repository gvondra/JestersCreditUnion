using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public class InterestRateConfigurationService : IInterestRateConfigurationService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public InterestRateConfigurationService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<InterestRateConfiguration> Get(ISettings settings)
        {
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("InterestRate")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.InterestRateConfiguration>(_service, request);
        }

        public Task<InterestRateConfiguration> Save(ISettings settings, InterestRateConfiguration interestRateConfiguration)
        {
            if (interestRateConfiguration == null)
                throw new ArgumentNullException(nameof(interestRateConfiguration));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put, interestRateConfiguration)
                .AddPath("InterestRate")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.InterestRateConfiguration>(_service, request);
        }
    }
}
