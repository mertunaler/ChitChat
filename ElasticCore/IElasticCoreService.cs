using ElasticCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticCore
{
    internal interface IElasticCoreService<T> where T : class
    {
        public IReadOnlyCollection<Chat> GetChatLog(int rowCount);
        public void InsertLog(T logMode, string indexName);
    }
}
