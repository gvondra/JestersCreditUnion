using Azure.Identity;
using Azure.Messaging.ServiceBus;
using JestersCreditUnion.Loan.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;

namespace JestersCreditUnion.Loan.Core
{
    public class ServiceBusService
    {
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            ContractResolver = new DefaultContractResolver()
        };
        internal static Task NewLoanApplicationEnqueue(ISettings settings, Guid loanApplicationId)
        {
            string body = JsonConvert.SerializeObject(new { LoanApplicationId = loanApplicationId }, _serializerSettings);
            return Enqueue(settings, settings.ServiceBusNewLoanAppQueue, body);
        }

        private static async Task Enqueue(ISettings settings, string queue, string body)
        {
            ServiceBusClient client = CreateClient(settings);
            ServiceBusSender sender = CreateSender(client, queue);
            try
            {
                await sender.SendMessageAsync(new ServiceBusMessage(body));
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }

        private static ServiceBusSender CreateSender(ServiceBusClient client, string queue)
            => client.CreateSender(queue);

        private static ServiceBusClient CreateClient(ISettings settings)
        {
            ServiceBusClientOptions options = new ServiceBusClientOptions
            {
                TransportType = ServiceBusTransportType.AmqpWebSockets
            };
            return new ServiceBusClient(settings.ServiceBusNamespace, new DefaultAzureCredential(), options);
        }
    }
}
