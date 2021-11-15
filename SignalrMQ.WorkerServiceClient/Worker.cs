using Microsoft.AspNetCore.SignalR.Client;
using SignalrMQ.Client;
using SignalrMQ.Client.Services;
using SignalrMQ.WorkerServiceClient.Configuration;

namespace SignalrMQ.WorkerServiceClient;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly SignalrMqClientService signalrMqClientService;
    private object connection;

    public Worker(ILogger<Worker> logger, SignalrMqClientService signalrMqClientService)
    {
        _logger = logger;
        this.signalrMqClientService = signalrMqClientService;
        this.signalrMqClientService.ConnectionEstablished += SignalrMqClientService_Connected;
        this.signalrMqClientService.MessageReceived += SignalrMqClientService_MessageReceived;
        this.signalrMqClientService.MessageResponseReceived += SignalrMqClientService_MessageResponseReceived;
    }

    private void SignalrMqClientService_MessageResponseReceived(object? sender, MessageReceivedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void SignalrMqClientService_MessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        _logger.LogInformation(e.MessageItem.Payload.ToString());
    }

    private void SignalrMqClientService_Connected(object? sender, EventArgs e)
    {
        Task.Run(async () =>
        {
                //await signalrMqClientService.Subscribe("testapikey", "testRequest");
                //await signalrMqClientService.SubscribeForResponse("testapikey", "archive");
            });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int i = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Sende..." + i);
            await signalrMqClientService.Publish("testapikey", "test", "testref", new TestObject(i, DateTime.Now, "TestPublish/with/path"));
            //await signalrMqClientService.PublishRequest("testapikey", "getDateTime", "testref", "Payload: " + DateTime.Now + " " + i);
            i++;
            await Task.Delay(1000, stoppingToken);
        }
    }
}

public record TestObject(int Counter, DateTime Timestamp, string Description);
