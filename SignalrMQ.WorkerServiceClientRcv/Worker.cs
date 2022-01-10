using SignalrMQ.Client;
using SignalrMQ.Client.Services;
using SignalrMQ.Core;
using SignalrMQ.WorkerServiceClientRcv.Configuration;
using System.Text.Json;

namespace SignalrMQ.WorkerServiceClientRcv;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly SignalrMqClientService signalrMqClientService;

    public Worker(ILogger<Worker> logger, SignalrMqClientService signalrMqClientService)
    {
        _logger = logger;
        this.signalrMqClientService = signalrMqClientService;
        this.signalrMqClientService.ConnectionEstablished += SignalrMqClientService_Connected;
        this.signalrMqClientService.MessageReceived += SignalrMqClientService_MessageReceived;
        this.signalrMqClientService.MessageRequestReceived += SignalrMqClientService_MessageRequestReceived;

        // just for demonstration, if Autoconnect == false, "StartConnection" can be triggered dedicated
        Task.Run(async () =>
        {
            await this.signalrMqClientService.StartConnection();
        });
    }

    private async void SignalrMqClientService_MessageRequestReceived(object? sender, MessageReceivedEventArgs e)
    {
        _logger.LogInformation($"Request Received {e.MessageItem.ExchangeName}");
        await signalrMqClientService.PublishResponse(e.MessageItem.ApiKey, e.MessageItem.ExchangeName, e.MessageItem.ReferenceCode, DateTime.Now);
    }

    private void SignalrMqClientService_Connected(object? sender, EventArgs e)
    {
        Task.Run(async () =>
        {
            await signalrMqClientService.Subscribe("testapikey", "test");
            await signalrMqClientService.SubscribeForRequest("testapikey", "getDateTime");
        });
    }

    private void SignalrMqClientService_MessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        _logger.LogInformation(e.MessageItem.ReferenceCode + " - " + e.MessageItem.Payload.ToString());

        TestObject rcved = JsonSerializer.Deserialize<TestObject>(e.MessageItem.Payload.ToString());


        if (e.MessageItem.ExchangeName == "testResponse")
        {
            Task.Run(async () =>
            {
                await GetDateTime(e.MessageItem);
            });
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!signalrMqClientService.IsConnected)
        {
            _logger.LogInformation("connecting...");

            await Task.Delay(1000, stoppingToken);
        }

    }

    private async Task GetDateTime(MessageItem mi)
    {
        // use payload for anything
        // ....
        //

        _logger.LogInformation("GetDateTime called!");

        await signalrMqClientService.Publish(mi.ApiKey, "testRequest", mi.ReferenceCode, DateTime.Now);
    }
}
public record TestObject(int Counter, DateTime Timestamp, string Description);
