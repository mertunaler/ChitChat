using RedisCore;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;

internal class Program
{
    static async Task Main(string[] args)
    {
        using (RedisClient _client = new RedisClient())
        {
            var pubSub = _client.GetSubscriber();
            bool isContinue = true;
            while (isContinue)
            {
                await pubSub.SubscribeAsync(_client.Channel2, (channel, message) =>
                {
                    Console.WriteLine(Environment.NewLine + "John: " + message);

                    Console.WriteLine("Type in your message: ");
                    string messageFromJohn = Console.ReadLine();

                    isContinue = messageFromJohn.ToLower().Trim() != "exit" ? true : false;

                    pubSub.PublishAsync(_client.Channel1, messageFromJohn, StackExchange.Redis.CommandFlags.FireAndForget);
                });

                Console.WriteLine("Type in your message: ");
                string message = Console.ReadLine();

                isContinue = message.ToLower().Trim() != "exit" ? true : false;

                await pubSub.PublishAsync(_client.Channel1, message, StackExchange.Redis.CommandFlags.FireAndForget);
            }
        }
    }
}