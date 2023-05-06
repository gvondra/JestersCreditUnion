﻿using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class LoanApplicationService : ILoanApplicationService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public LoanApplicationService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<LoanApplicationComment> AppendComent(ISettings settings, Guid id, LoanApplicationComment comment, bool isPublic = false)
        {
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(comment.Text))
                throw new ApplicationException("Comment Text is required");
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post, comment)
                .AddPath("LoanApplication/{id}/Comment")
                .AddPathParameter("id", id.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            if (isPublic)
                request.AddQueryParameter("isPublic", isPublic.ToString());
            return _restUtil.Send<LoanApplicationComment>(_service, request);
        }

        public Task<LoanApplication> Get(ISettings settings, Guid id)
        {
            if (id.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(id));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("LoanApplication/{id}")
                .AddPathParameter("id", id.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<LoanApplication>(_service, request);
        }

        public Task<LoanApplication> Update(ISettings settings, LoanApplication loanApplication)
        {
            if (loanApplication == null)
                throw new ArgumentNullException(nameof(loanApplication));
            if (!loanApplication.LoanApplicationId.HasValue || loanApplication.LoanApplicationId.Value.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(LoanApplication.LoanApplicationId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put, loanApplication)
                .AddPath("LoanApplication/{id}")
                .AddPathParameter("id", loanApplication.LoanApplicationId.Value.ToString("N"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<LoanApplication>(_service, request);
        }
    }
}
