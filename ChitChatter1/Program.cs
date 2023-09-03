using RedisCore;

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

                await pubSub.SubscribeAsync(_client.Channel1, (channel, message) =>
                {
                    Console.WriteLine(Environment.NewLine + "Jane: " + message);

                    Console.WriteLine("Type in your message: ");
                    string messageFromJane = Console.ReadLine();

                    isContinue = messageFromJane.ToLower().Trim() != "exit" ? true : false;

                    pubSub.PublishAsync(_client.Channel2, messageFromJane, StackExchange.Redis.CommandFlags.FireAndForget);
                });

                Console.WriteLine("Type in your message: ");
                string message = Console.ReadLine();

                isContinue = message.ToLower().Trim() != "exit" ? true : false;

                await pubSub.PublishAsync(_client.Channel2, message, StackExchange.Redis.CommandFlags.FireAndForget);
            }
        }
    }
}