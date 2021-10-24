using Microsoft.AspNetCore.SignalR;

namespace SignalrMQ.Broker
{
    public class SignalrMqBroker : Hub, ISignalrMqBroker
    {
        public Task Publish(string exchangename, object obj)
        {
            throw new NotImplementedException();
        }

        public Task Subscribe(string exchangename)
        {
            throw new NotImplementedException();
        }
    }
}