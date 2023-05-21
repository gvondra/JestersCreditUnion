﻿using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
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

        public Task<Loan> Create(ISettings settings, Loan loan)
        {
            if (loan == null)
                throw new ArgumentNullException(nameof(loan));
            if (loan.Agreement == null)
                throw new ArgumentNullException(nameof(loan.Agreement));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post, loan)
                .AddPath("Loan")
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Loan>(_service, request);
        }
    }
}