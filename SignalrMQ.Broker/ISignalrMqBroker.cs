
namespace SignalrMQ.Broker
{
    public interface ISignalrMqBroker
    {
        Task Publish(string apiKey, string exchangename, string referenceCode, object obj);
        Task Subscribe(string apiKey, string exchangename);
        Task Unsubscribe(string apiKey, string exchangename);
    }
}