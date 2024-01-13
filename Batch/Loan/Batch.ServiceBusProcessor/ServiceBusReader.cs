using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace JestersCreditUnion.Batch.ServiceBusProcessor
{
    public class ServiceBusReader : IServiceBusReader
    {
        private readonly Settings _settings;
        private ILogger<ServiceBusReader> _logger;
        private ServiceBusClient _serviceBusClient;
        private Azure.Messaging.ServiceBus.ServiceBusProcessor _serviceBusProcessor;
        private IMesssageHandler _messsageHandler;

        public ServiceBusReader(Settings settings, ILogger<ServiceBusReader> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public Task StartProcessing(string queue, IMesssageHandler messsageHandler)
        {
            _messsageHandler = messsageHandler;
            if (_serviceBusClient == null && _serviceBusProcessor == null)
            {
                _serviceBusClient = CreateClient(_settings);
                _serviceBusProcessor = CreateProcesor(_serviceBusClient, queue);
                _serviceBusProcessor.ProcessErrorAsync += ProcessErrorAsync;
                _serviceBusProcessor.ProcessMessageAsync += ProcessMessageAsync;
                return _serviceBusProcessor.StartProcessingAsync();
            }
            return Task.CompletedTask;
        }

        public async Task StopProcessing()
        {
            if (_serviceBusProcessor != null)
                await _serviceBusProcessor.StopProcessingAsync();
            await DisposeAsync();
        }

        private async Task ProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            _logger.LogTrace("Received new loan application request");
            _logger.LogTrace(arg.Message.Body.ToString());
            await _messsageHandler?.Process(arg.Message.Body.ToString());
            await arg.CompleteMessageAsync(arg.Message);
            //await arg.AbandonMessageAsync(arg.Message);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, arg.Exception.Message);
            return Task.CompletedTask;
        }

        private static Azure.Messaging.ServiceBus.ServiceBusProcessor CreateProcesor(ServiceBusClient serviceBusClient, string queue)
            => serviceBusClient.CreateProcessor(queue);

        private static ServiceBusClient CreateClient(Settings settings)
        {
            ServiceBusClientOptions options = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            return new ServiceBusClient(settings.ServiceBusNamespace, new DefaultAzureCredential(), options);
        }

        public async Task DisposeAsync()
        {
            if (_serviceBusProcessor != null)
                await _serviceBusProcessor.DisposeAsync();
            _serviceBusProcessor = null;
            if (_serviceBusClient != null)
                await _serviceBusClient.DisposeAsync();
            _serviceBusClient = null;
        }
    }
}
