using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticCore.Model
{
    public class Chat
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Message { get; set; }
        public DateTime CreDate { get; set; }
    }
}
