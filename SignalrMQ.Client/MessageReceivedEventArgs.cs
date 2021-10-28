namespace SignalrMQ.Client
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public string ReferenceCode { get; set; }
        public object Payload { get; set; }
    }
}