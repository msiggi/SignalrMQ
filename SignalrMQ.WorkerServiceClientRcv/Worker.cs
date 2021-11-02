using SignalrMQ.Client;
using SignalrMQ.WorkerServiceClientRcv.Configuration;

namespace SignalrMQ.WorkerServiceClientRcv
{
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
        }

        private void SignalrMqClientService_Connected(object? sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                await signalrMqClientService.Subscribe("testapikey", "test");
            });
        }

        private void SignalrMqClientService_MessageReceived(object? sender, MessageReceivedEventArgs e)
        {
            _logger.LogInformation(e.ReferenceCode + " - " + e.Payload.ToString());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!signalrMqClientService.IsConnected)
            {
                _logger.LogInformation("connecting...");

                await Task.Delay(1000, stoppingToken);
            }

        }
    }
}