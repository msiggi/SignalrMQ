
namespace SignalrMQ.Broker
{
    public interface ISignalrMqBroker
    {
        Task Publish(string exchangename, object obj);
        Task Subscribe(string exchangename);
    }
}