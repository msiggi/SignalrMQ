using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalrMQ.Core;

namespace SignalrMQ.Client;

public class SignalrMqClientService : ISignalrMqClientService
{
    private readonly ILogger<SignalrMqClientService> logger;
    private HubConnection hubConnection;
    public event EventHandler<MessageReceivedEventArgs> MessageReceived;
    public event EventHandler<MessageReceivedEventArgs> MessageRequestReceived;
    public event EventHandler<MessageReceivedEventArgs> MessageResponseReceived;
    public event EventHandler<EventArgs> ConnectionEstablished;

    public SignalrMqClientService(ILogger<SignalrMqClientService> logger, IOptions<SignalrMqEndpoint> options)
    {
        logger.LogInformation("SignalrMqClientService initalized!");
        this.logger = logger;
        Task.Run(async () =>
        {
            if (options.Value.Host == null || options.Value.Port == null)
            {
                logger.LogError("Host and/or Port are null. No connections possible!");
            }
            await StartConnection(options.Value.Host, options.Value.Port);//.GetAwaiter().GetResult();
        });
    }

    public async Task StartConnection(string host, int port = 443)
    {
        string url = $"https://{host}:{port}/signalrmqbrokerhub";
        hubConnection = new HubConnectionBuilder()
                        .WithUrl(url)
                        .Build();

        hubConnection.Closed += async (error) =>
        {
            logger.LogError($"Connection to {url} closed! Trying to reconnect...");

            await Task.Delay(new Random().Next(0, 5) * 1000);
            await hubConnection.StartAsync();
        };

        hubConnection.On<MessageItem>("rcv_request", rcv =>
        {
            rcv.ExchangeName = rcv.ExchangeName.Replace("__request", "");
            MessageRequestReceived?.Invoke(this, new MessageReceivedEventArgs
            {
                MessageItem = rcv
            });
        });

        hubConnection.On<MessageItem>("rcv_response", rcv =>
        {
            rcv.ExchangeName = rcv.ExchangeName.Replace("__response", "");
            MessageResponseReceived?.Invoke(this, new MessageReceivedEventArgs
            {
                MessageItem = rcv
            });
        });

        hubConnection.On<MessageItem>("rcv", rcv =>
        {
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs
            {
                MessageItem = rcv
            });
        });

        await Connect(url);
    }

    private async Task Connect(string url)
    {
        try
        {
            logger.LogInformation($"connecting to {url} ...");

            await hubConnection.StartAsync().ContinueWith((t) =>
            {
                ConnectionEstablished?.Invoke(this, new EventArgs());
                logger.LogInformation($"Connection to {url} established!");
            });

        }
        catch (Exception ex)
        {
            logger.LogError($"No connection to {url} established! Trying again...");

            await Task.Delay(new Random().Next(0, 5) * 1000);
            await Connect(url);
        }
    }

    public async Task Publish(string apiKey, string exchangename, string referenceCode, object payload)
    {
        if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
        {
            await hubConnection.SendAsync("Publish", apiKey, exchangename, referenceCode, payload);
        }
    }

    public async Task PublishRequest(string apiKey, string exchangename, string referenceCode, object payload)
    {
        if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
        {
            await hubConnection.SendAsync("PublishRequest", apiKey, exchangename, referenceCode, payload);
        }
    }

    public async Task PublishResponse(string apiKey, string exchangename, string referenceCode, object payload)
    {
        if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
        {
            await hubConnection.SendAsync("PublishResponse", apiKey, exchangename, referenceCode, payload);
        }
    }

    public async Task Subscribe(string apiKey, string exchangename)
    {
        if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
        {
            await hubConnection.SendAsync("Subscribe", apiKey, exchangename);
        }
    }

    public async Task SubscribeForResponse(string apiKey, string exchangename)
    {
        exchangename += "__response";
        await Subscribe(apiKey, exchangename);
    }

    public async Task SubscribeForRequest(string apiKey, string exchangename)
    {
        exchangename += "__request";
        await Subscribe(apiKey, exchangename);
    }

    public async Task Unsubscribe(string apiKey, string exchangename)
    {
        if (hubConnection != null && hubConnection.State == HubConnectionState.Connected)
        {
            await hubConnection.SendAsync("Unsubscribe", apiKey, exchangename);
        }
    }

    public bool IsConnected
    {
        get { return hubConnection != null && hubConnection.State == HubConnectionState.Connected; }
    }
}