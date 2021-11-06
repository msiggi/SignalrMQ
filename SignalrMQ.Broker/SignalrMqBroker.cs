using Microsoft.AspNetCore.SignalR;
using SignalrMQ.Core;

namespace SignalrMQ.Broker;

public class SignalrMqBroker : Hub
{
    public async Task Publish(string apiKey, string exchangename, string referenceCode, object payload)
    {
        string? gk = GetGroupKey(apiKey, exchangename);
        await Clients.Group(gk).SendAsync("rcv", new MessageItem(apiKey, exchangename, referenceCode, payload));
    }
    public async Task PublishRequest(string apiKey, string exchangename, string referenceCode, object payload)
    {
        exchangename += "_request";

        await AddToGroup(apiKey, exchangename);

        string? gk = GetGroupKey(apiKey, exchangename);
        await Clients.Group(gk).SendAsync("rcv_request", new MessageItem(apiKey, exchangename, referenceCode, payload));
    }
    public async Task PublishResponse(string apiKey, string exchangename, string referenceCode, object payload)
    {
        exchangename += "_request";

        string? gk = GetGroupKey(apiKey, exchangename);
        await Clients.Group(gk).SendAsync("rcv_response", new MessageItem(apiKey, exchangename, referenceCode, payload));
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
