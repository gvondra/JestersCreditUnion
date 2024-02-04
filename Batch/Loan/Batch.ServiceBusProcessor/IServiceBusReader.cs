using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public interface IServiceBusReader
    {
        Task StartProcessing(string queue, IMesssageHandler messsageHandler);
        Task StopProcessing();
        Task DisposeAsync();
    }
}
