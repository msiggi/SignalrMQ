using Microsoft.AspNetCore.SignalR;
using SignalrMQ.Core;

namespace SignalrMQ.Broker;

public class SignalrMqBroker : Hub, ISignalrMqBroker
{
    public async Task Publish(string apiKey, string exchangename, string referenceCode, object payload)
    {
        await AddToGroup(apiKey, exchangename);
        
        string? gk = GetGroupKey(apiKey, exchangename);
        await Clients.Group(gk).SendAsync("rcv", new MessageItem(referenceCode, payload));
    }

    public async Task Subscribe(string apiKey, string exchangename)
    {
        await AddToGroup(apiKey, exchangename);
    }

    public async Task Unsubscribe(string apiKey, string exchangename)
    {
        await RemoveFromGroup(apiKey, exchangename);
    }

    private async Task AddToGroup(string apiKey, string exchangename)
    {
        string? gk = GetGroupKey(apiKey, exchangename);
        await Groups.AddToGroupAsync(Context.ConnectionId, gk);
    }

    private async Task RemoveFromGroup(string apiKey, string exchangename)
    {
        string? gk = GetGroupKey(apiKey, exchangename);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gk);
    }

    private string GetGroupKey(string apiKey, string groupName)
    {
        return $"{apiKey}_{groupName}";
    }
}
