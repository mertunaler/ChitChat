﻿using ElasticCore;
using ElasticCore.Model;
using RedisCore;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;

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
                await pubSub.SubscribeAsync(_client.Channel2, (channel, message) =>
                {
                    Console.WriteLine(Environment.NewLine + "John: " + message);

                    Console.WriteLine("Type in your message: ");
                    string messageFromJohn = Console.ReadLine();

                    isContinue = messageFromJohn.ToLower().Trim() != "exit" ? true : false;

                    pubSub.PublishAsync(_client.Channel1, messageFromJohn, StackExchange.Redis.CommandFlags.FireAndForget);
                    Chat chatModel = new Chat() { From = "John", To = "Jane", Message = messageFromJohn, CreDate = DateTime.Now };
                    elastic.InsertLog(chatModel, "redisLog");

                });

                Console.WriteLine("Type in your message: ");
                string message = Console.ReadLine();

                isContinue = message.ToLower().Trim() != "exit" ? true : false;

                await pubSub.PublishAsync(_client.Channel1, message, StackExchange.Redis.CommandFlags.FireAndForget);
                Chat chatModel = new Chat() { From = "John", To = "Jane", Message = message, CreDate = DateTime.Now };
                elastic.InsertLog(chatModel, "redisLog");
            }
        }
    }
}