using SignalrMQ.Core;

namespace SignalrMQ.Client.Services
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageItem MessageItem { get; set; }
    }
}