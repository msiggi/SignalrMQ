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
        // automatically subscripe for response
        string exchangename_Response = String.Concat(exchangename, "__response");
        await AddToGroup(apiKey, exchangename_Response);

        string exchangename_Request = String.Concat(exchangename, "__request");
        string? gk = GetGroupKey(apiKey, exchangename_Request);
        await Clients.Group(gk).SendAsync("rcv_request", new MessageItem(apiKey, exchangename_Request, referenceCode, payload));
    }
    public async Task PublishResponse(string apiKey, string exchangename, string referenceCode, object payload)
    {
        exchangename += "__response";

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
