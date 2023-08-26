using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class LoanPaymentService : ILoanPaymentService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public LoanPaymentService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }
        public Task<List<LoanPayment>> Save(ISettings settings, IEnumerable<LoanPayment> loanPayments)
        {
            if (loanPayments == null)
                throw new ArgumentNullException(nameof(loanPayments));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post)
                .AddPath("Loan/Payment")
                .AddJsonBody(loanPayments)
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<LoanPayment>>(_service, request);
        }
    }
}
