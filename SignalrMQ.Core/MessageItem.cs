using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrMQ.Core
{
    public class MessageItem
    {
        public MessageItem(string referenceCode, object payload)
        {
            ReferenceCode = referenceCode;
            Payload = payload;
        }

        public string ReferenceCode { get; set; }
        public object Payload { get; set; }
    }
}
