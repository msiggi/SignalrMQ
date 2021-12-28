using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrMQ.Client.Configuration
{
    public class SignalrMqEndpoint
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Autoconnect { get; set; } = true;
    }
}
