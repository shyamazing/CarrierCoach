using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace SBSender
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
            Console.WriteLine("Enter message to send. type 'exit' to exit the application");
            bool exit = false;
            do
            {
                // Prompt user to enter a message
                string message = Console.ReadLine()!;

                // Check if the user wants to exit the application
                if (message.Trim().ToLower() == "exit")
                {
                    exit = true;
                }
                // Check if the message is not empty
                else if (!string.IsNullOrEmpty(message))
                {
                    // Send the message to the queue
                    await SendMessage(message);
                }
                else
                {
                    Console.WriteLine("Message cannot be empty");
                }
            }
            while (!exit);
        }

        // Method to send a message to the Azure Service Bus queue
        public static async Task SendMessage(string message)
        {
            // Create a Service Bus client
            ServiceBusClient serviceBusClient = new ServiceBusClient(_connectionString);

            // Create a sender for the queue
            ServiceBusSender sender = serviceBusClient.CreateSender(_queueName);

            // Serialize the message to JSON format
            string messageBody = JsonSerializer.Serialize(message);

            // Create a Service Bus message with the serialized message body
            ServiceBusMessage messageContent = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));

            // Send the message asynchronously
            await sender.SendMessageAsync(messageContent);

            // Dispose the sender and client asynchronously
            await sender.DisposeAsync();
            await serviceBusClient.DisposeAsync();
        }
    }
}
