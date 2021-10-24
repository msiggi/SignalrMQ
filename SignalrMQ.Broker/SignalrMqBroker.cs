using Microsoft.AspNetCore.SignalR;

namespace SignalrMQ.Broker
{
    public class SignalrMqBroker : Hub
    {
        public Task Publish(string exchangename, object obj)
        {

        }
    }
}