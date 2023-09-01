using BrassLoon.RestClient;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace JestersCreditUnion.Interface
{
    public class IdentificationCardService : IIdentificationCardService
    {
        private readonly RestUtil _restUtil;
        private readonly IService _service;

        public IdentificationCardService(RestUtil restUtil, IService service)
        {
            _restUtil = restUtil;
            _service = service;
        }

        public async Task<Stream> GetBorrowerIdentificationCard(ISettings settings, Guid loanApplicationId)
        {
            if (loanApplicationId.Equals(Guid.Empty))
                throw new ArgumentNullException(nameof(loanApplicationId));
            IRequest request = _service.CreateRequest(new Uri(settings.BaseAddress), HttpMethod.Get)
                .AddPath("LoanApplication/{id}/BorrowerIdentificationCard")
                .AddPathParameter("id", loanApplicationId.ToString("D"))
                .AddJwtAuthorizationToken(settings.GetToken)
                ;
            IResponse response = await _service.Send(request);
            _restUtil.CheckSuccess(response);
            return await response.Message.Content.ReadAsStreamAsync();
        }
    }
}
