using BrassLoon.RestClient;
using JestersCreditUnion.Interface.Loan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface.Loan
{
    public class PaymentIntakeService : IPaymentIntakeService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public PaymentIntakeService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public Task<PaymentIntake> Create(ISettings settings, PaymentIntake paymentIntake)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            else if (paymentIntake == null || !paymentIntake.LoanId.HasValue)
                throw new ArgumentException($"Missing {nameof(paymentIntake.LoanId)} value");
            else if (!paymentIntake.Amount.HasValue)
                throw new ArgumentException($"Missing {nameof(paymentIntake.Amount)} value");
            else if (!paymentIntake.Date.HasValue)
                throw new ArgumentException($"Missing {nameof(paymentIntake.Date)} value");
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Post, paymentIntake)
                .AddPath("Loan/{loanId}/PaymentIntake")
                .AddPathParameter("loanId", paymentIntake.LoanId.Value.ToString("D"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.PaymentIntake>(_service, request);
        }

        public Task<List<PaymentIntake>> GetByStatuses(ISettings settings, IEnumerable<short> statuses)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            else if (statuses == null)
                throw new ArgumentNullException(nameof(statuses));
            string address = _restUtil.AppendPath(settings.BaseAddress, "PaymentIntake");
            address += "?" + string.Join("&", statuses.Select(s => $"status={s}"));
            IRequest request = _service.CreateRequest(new Uri(address), HttpMethod.Get)
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<List<Models.PaymentIntake>>(_service, request);
        }

        public Task<PaymentIntake> Update(ISettings settings, PaymentIntake paymentIntake)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            else if (paymentIntake == null || !paymentIntake.PaymentId.HasValue)
                throw new ArgumentException($"Missing {nameof(paymentIntake.PaymentId)} value");
            else if (!paymentIntake.Amount.HasValue)
                throw new ArgumentException($"Missing {nameof(paymentIntake.Amount)} value");
            else if (!paymentIntake.Date.HasValue)
                throw new ArgumentException($"Missing {nameof(paymentIntake.Date)} value");
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Put, paymentIntake)
                .AddPath("PaymentIntake/{id}")
                .AddPathParameter("id", paymentIntake.PaymentIntakeId.Value.ToString("D"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            return _restUtil.Send<Models.PaymentIntake>(_service, request);
        }
    }
}
