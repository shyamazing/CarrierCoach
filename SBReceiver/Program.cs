using Azure.Messaging.ServiceBus;
using System.Text;

namespace SBReceiver
{
    internal class Program
    {
        // use azure service bus emulator provided by azure without actual azure subscription
        // https://github.com/Azure/azure-service-bus-emulator-installer
        // Connection string to the Azure Service Bus namespace
        private static string _connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";

        // Name of the queue to send messages to
        private static string _queueName = "queue.1";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Waiting for messages...");

            // Start receiving messages
            ReceiveMessage();

            // Wait for user input to exit
            Console.ReadLine();
        }

        public static async void ReceiveMessage()
        {
            // create bus client
            ServiceBusClient serviceBusClient = new ServiceBusClient(_connectionString);

            // configure options for receiver to peek lock
            ServiceBusReceiverOptions options = new ServiceBusReceiverOptions()
            {
                ReceiveMode = ServiceBusReceiveMode.PeekLock
            };

            // create receiver
            ServiceBusReceiver receiver = serviceBusClient.CreateReceiver(_queueName, options);
            while (true)
            {
                // read message
                ServiceBusReceivedMessage message = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5));
                if (message != null)
                {
                    // Print received message
                    Console.WriteLine($"Received message: {Encoding.UTF8.GetString(message.Body)}");

                    // Complete the message so it is not received again
                    await receiver.CompleteMessageAsync(message);
                }
            }
        }
    }
}
