using ElasticCore.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticCore
{
    public class ElasticCoreService<T> : IDisposable,IElasticCoreService<T> where T : class
    {
        private bool Disposed;

        public void Dispose()
        {
            Disposed = true;
        }

        public IReadOnlyCollection<Chat> GetChatLog(int rowCount)
        {
            using (ElasticClientManager provider = new ElasticClientManager())
            {
                ElasticClient _client = provider._client;
                string indexName = "redisLog";
                var response = _client.Search<Chat>(s => s
                .Size(rowCount)
                .Sort(ss => ss.Descending(p => p.CreDate))
                .Index(indexName)
                );
                return response.Documents;
            }
        }

        public void InsertLog(T logMode, string indexName)
        {
            using (ElasticClientManager _manager = new ElasticClientManager())
            {
                ElasticClient _client = _manager._client;

                if (!_client.Indices.Exists(indexName).Exists)
                {
                    var nIndexName = string.Concat(indexName, DateTime.Now.Ticks);

                    var indexSettings = new IndexSettings()
                    {
                        NumberOfReplicas = 1,
                        NumberOfShards = 3,
                    };

                    var response = _client.Indices.Create(nIndexName, index => index.Map<T>(m => m.AutoMap())
                                                          .InitializeUsing(new IndexState() { Settings = indexSettings })
                                                          .Aliases(als => als.Alias(indexName))
                                                          );

                }
                IndexResponse responseIndex = _client.Index<T>(logMode, idx => idx.Index(indexName));

            }

        }
    }


}
