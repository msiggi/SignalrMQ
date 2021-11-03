using SignalrMQ.Core;

namespace SignalrMQ.Client
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageItem MessageItem { get; set; }
    }
}