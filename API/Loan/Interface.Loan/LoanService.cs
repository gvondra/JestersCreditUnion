using BrassLoon.RestClient;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public class LoanService : ILoanService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public LoanService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<Models.Loan> Create(ISettings settings, Models.Loan loan)
        {
            if (loan == null)
                throw new ArgumentNullException(nameof(loan));
            if (loan.Agreement == null)
                throw new ArgumentException($"{nameof(loan.Agreement)} is null");
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post, loan)
                .AddPath("Loan")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.Loan>(_service, request);
        }

        public Task<Models.Loan> InitiateDisbursement(ISettings settings, Guid id)
        {
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(id));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post)
                .AddPath("Loan/{id}/InitiateDisbursement")
                .AddPathParameter("id", id.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.Loan>(_service, request);
        }

        public Task<Models.Loan> Get(ISettings settings, Guid id)
        {
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(id));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("Loan/{id}")
                .AddPathParameter("id", id.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.Loan>(_service, request);
        }

        public Task<List<Models.Loan>> GetByBorrowerNameBirthDate(ISettings settings, string borrowerName, DateTime borrowerBirthDate)
        {
            if (string.IsNullOrEmpty(borrowerName))
                throw new ArgumentNullException(nameof(borrowerName));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("Loan")
                .AddQueryParameter("borrowerName", borrowerName)
                .AddQueryParameter("borrowerBirthDate", borrowerBirthDate.ToString("O"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<Models.Loan>>(_service, request);
        }

        public Task<Models.Loan> GetByLoanApplicationId(ISettings settings, Guid loanApplicationId)
        {
            if (loanApplicationId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(loanApplicationId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("Loan")
                .AddQueryParameter("loanApplicationId", loanApplicationId.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.Loan>(_service, request);
        }

        public Task<Models.Loan> GetByNumber(ISettings settings, string number)
        {
            if (string.IsNullOrEmpty(number))
                throw new ArgumentNullException(nameof(number));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("Loan")
                .AddQueryParameter("number", number)
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.Loan>(_service, request);
        }

        public Task<Models.Loan> Update(ISettings settings, Models.Loan loan)
        {
            if (loan == null)
                throw new ArgumentNullException(nameof(loan));
            if (loan.Agreement == null)
                throw new ArgumentException($"{nameof(loan.Agreement)} is null");
            if (!loan.LoanId.HasValue || loan.LoanId.Value.Equals(Guid.Empty))
                throw new ArgumentException($"{nameof(loan.LoanId)} is null");
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put, loan)
                .AddPath("Loan/{id}")
                .AddPathParameter("id", loan.LoanId.Value.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.Loan>(_service, request);
        }

        public Task<Models.DisburseResponse> DisburseFunds(ISettings settings, Guid id)
        {
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(id));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post)
                .AddPath("Loan/{id}/Disbursement")
                .AddPathParameter("id", id.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.DisburseResponse>(_service, request);
        }
    }
}
