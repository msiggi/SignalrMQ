using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrMQ.Core
{
    public class MessageItem
    {
        public MessageItem(string apiKey, string exchangeName, string referenceCode, object payload)
        {
            ApiKey = apiKey;
            ExchangeName = exchangeName;
            ReferenceCode = referenceCode;
            Payload = payload;
        }
        public string ApiKey { get; set; }
        public string ExchangeName { get; set; }
        public string ReferenceCode { get; set; }
        public object Payload { get; set; }
    }
}
