using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public class AmortizationService : IAmortizationService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public AmortizationService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<List<AmortizationItem>> Get(ISettings settings, Guid loanId)
        {
            if (loanId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(loanId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("Loan/{id}/Amortization")
                .AddPathParameter("id", loanId.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<AmortizationItem>>(_service, request);
        }
    }
}
