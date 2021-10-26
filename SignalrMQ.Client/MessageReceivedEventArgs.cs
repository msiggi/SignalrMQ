namespace SignalrMQ.Client
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public object Payload { get; set; }
    }
}