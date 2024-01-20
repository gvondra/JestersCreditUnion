using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public class LoanApplicationRatingService : ILoanApplicationRatingService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public LoanApplicationRatingService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<Rating> Create(ISettings settings, Guid loanApplicationId)
        {
            if (loanApplicationId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(loanApplicationId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post)
                .AddPath("LoanApplication/{id}/Rating")
                .AddPathParameter("id", loanApplicationId.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Rating>(_service, request);
        }

        public Task<Rating> Get(ISettings settings, Guid loanApplicationId)
        {
            if (loanApplicationId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(loanApplicationId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("LoanApplication/{id}/Rating")
                .AddPathParameter("id", loanApplicationId.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Rating>(_service, request);
        }
    }
}
