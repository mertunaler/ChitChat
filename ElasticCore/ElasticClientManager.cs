using Nest;
using System.Configuration;

namespace ElasticCore
{
    public class ElasticClientManager : IDisposable
    {
        private bool Disposed;
        public ElasticClient _client;

        public ElasticClientManager()
        {
            _client = CreateElasticClient();

        }

        private ElasticClient CreateElasticClient()
        {
            var config = new ConnectionSettings(new Uri("http://localhost:1881")).BasicAuthentication("younevergonnaguessit", "Younevergonnafindit")
                                                                           .DisablePing()
                                                                           .DisableDirectStreaming(true)
                                                                           .SniffOnStartup(false)
                                                                           .SniffOnConnectionFault(false);

            return new ElasticClient(config);
        }

        public void Dispose()
        {
            Disposed = true;
        }
    }
}