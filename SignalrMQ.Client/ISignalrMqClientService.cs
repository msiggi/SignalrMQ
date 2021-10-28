
namespace SignalrMQ.Client
{
    public interface ISignalrMqClientService
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        Task Publish(string apiKey, string exchangename, string referenceCode, object payload);
        Task StartConnection(string host, int port = 443);
        Task Subscribe(string apiKey, string exchangename);
        Task Unsubscribe(string apiKey, string exchangename);
    }
}