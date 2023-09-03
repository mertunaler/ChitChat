using ElasticCore;
using ElasticCore.Model;
using RedisCore;

internal class Program
{
    static async Task Main(string[] args)
    {
        using (RedisClient _client = new RedisClient())
        using (ElasticCoreService<Chat> elastic = new ElasticCoreService<Chat>())
        {
            //Get Chat History From Elastic
            var chats = elastic.GetChatLog(5);
            Console.WriteLine("Previously on this chat.....");
            Console.WriteLine();
            foreach (var chat in chats)
            {
                Console.WriteLine($"-{chat.From}({chat.CreDate}): {chat.Message}");
            }
            Console.WriteLine();
            Console.WriteLine();
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
                    Chat chatModel = new Chat() { From = "Jane", To = "John", Message = messageFromJane, CreDate = DateTime.Now };
                    elastic.InsertLog(chatModel, "redisLog");
                });

                Console.WriteLine("Type in your message: ");
                string message = Console.ReadLine();

                isContinue = message.ToLower().Trim() != "exit" ? true : false;

                await pubSub.PublishAsync(_client.Channel2, message, StackExchange.Redis.CommandFlags.FireAndForget);
                Chat chatModel = new Chat() { From = "Jane", To = "John", Message = message, CreDate = DateTime.Now };
                elastic.InsertLog(chatModel, "redisLog");
            }
        }
    }
}