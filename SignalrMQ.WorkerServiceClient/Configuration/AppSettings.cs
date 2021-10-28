using SignalrMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalrMQ.WorkerServiceClient.Configuration
{
    public static class AppSettings
    {
        public static SignalrMqBrokerInformation BrokerSettings { get; set; }
    }
}
