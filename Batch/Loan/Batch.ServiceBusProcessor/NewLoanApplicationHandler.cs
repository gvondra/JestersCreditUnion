using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public class NewLoanApplicationHandler : IMesssageHandler
    {
        public Task Process(string messageBody)
        {
            throw new NotImplementedException();
        }
    }
}
