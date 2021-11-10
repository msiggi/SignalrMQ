using SignalrMQ.Client;
using SignalrMQ.Client.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrMQ.WorkerServiceClientRcv.Configuration
{
    public static class AppSettings
    {
        public static SignalrMqEndpoint BrokerSettings { get; set; }
    }
}
