using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public interface IMesssageHandler
    {
        Task Process(string messageBody);
    }
}
