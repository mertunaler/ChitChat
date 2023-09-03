using StackExchange.Redis;
using System.ComponentModel;
using System.Configuration;

namespace RedisCore
{
    public class RedisClient : IDisposable
    {
        public string Channel1 = "John Doe";
        public string Channel2 = "Jane Doe";
        public ConnectionMultiplexer _conn;
        private bool Disposed;
        public RedisClient()
        {
            var config = new ConfigurationOptions()
            {
                EndPoints = { "localhost:1923" },
                Password = "Younevergonnafindit", 
            };
            //Connection to the Redis Server.
            _conn = ConnectionMultiplexer.Connect(config);  
        }


        public ISubscriber GetSubscriber()
        {
            //Get a connection 
            return _conn.GetSubscriber();
        }

        public void Dispose()
        {
            Disposed = true;
        }
    }
}