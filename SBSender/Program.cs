using Azure.Messaging.ServiceBus;
using System.Text;
using System.Text.Json;

namespace SBSender
{
    internal class Program
    {
        private static string _connectionString = "Endpoint=sb://127.0.0.1;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=SAS_KEY_VALUE;UseDevelopmentEmulator=true;";
        private static string _queueName = "queue.1";
        static void Main(string[] args)
        {
            Console.WriteLine("Enter message to send. type 'exit' to exit the application");
            bool exit = false;
            do
            {
                Console.WriteLine("Enter message to send. type 'exit' to exit the application");
                string message = Console.ReadLine()!;
                if (message.Trim().ToLower() == "exit")
                {
                    exit = true;
                }
                else if (!string.IsNullOrEmpty(message))
                {
                    SendMessage(message);
                }
                else
                {
                    Console.WriteLine("Message cannot be empty");
                }
            }
            while (!exit);
        }
        public static void SendMessage(string message)
        {
            ServiceBusClient serviceBusClient = new ServiceBusClient(_connectionString);
            ServiceBusSender sender = serviceBusClient.CreateSender(_queueName);
            string messageBody = JsonSerializer.Serialize(message);
            ServiceBusMessage messageContent = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));
            sender.SendMessageAsync(messageContent);
            sender.DisposeAsync();
            serviceBusClient.DisposeAsync();
        }
    }
}
