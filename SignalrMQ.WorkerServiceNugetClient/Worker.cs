using SignalrMQ.Client;
using SignalrMQ.Client.Services;

namespace SignalrMQ.WorkerServiceNugetClient
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly SignalrMqClientService signalrMqClientService;

        public Worker(ILogger<Worker> logger, SignalrMqClientService signalrMqClientService)
        {
            _logger = logger;
            this.signalrMqClientService = signalrMqClientService;
            this.signalrMqClientService.ConnectionEstablished += SignalrMqClientService_ConnectionEstablished;
            this.signalrMqClientService.MessageReceived += SignalrMqClientService_MessageReceived;
        }

        private void SignalrMqClientService_MessageReceived(object? sender, MessageReceivedEventArgs e)
        {
            _logger.LogInformation(e.MessageItem.ReferenceCode + " - " + e.MessageItem.Payload.ToString());
        }

        private void SignalrMqClientService_ConnectionEstablished(object? sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                await signalrMqClientService.Subscribe("ThingsBridge", "ThingValue");
                // await signalrMqClientService.SubscribeForRequest("testapikey", "getDateTime");
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await signalrMqClientService.PublishRequest("ThingsBridge", "ThingValue", "", DateTime.Now);
                //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}