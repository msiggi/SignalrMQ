using Microsoft.AspNetCore.SignalR;

namespace SignalrMQ.Broker
{
    public class SignalrMqBroker : Hub, ISignalrMqBroker
    {
        public async Task Publish(string exchangename, object payload)
        {
            await AddToGroup(exchangename);
            await Clients.Group(exchangename).SendAsync("rcv", payload);
        }

        public async Task Subscribe(string exchangename)
        {
            await AddToGroup(exchangename);
        }

        public async Task Unsubscribe(string exchangename)
        {
            await RemoveFromGroup(exchangename);
        }

        private async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        private async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}