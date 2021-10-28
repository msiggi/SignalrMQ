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
            this.signalrMqClientService.StartConnection(AppSettings.BrokerSettings.Host, AppSettings.BrokerSettings.Port).GetAwaiter().GetResult();
            this.signalrMqClientService.MessageReceived += SignalrMqClientService_MessageReceived;
        }

        private void SignalrMqClientService_MessageReceived(object? sender, MessageReceivedEventArgs e)
        {
            _logger.LogInformation(e.ReferenceCode + " - " + e.Payload.ToString());
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await signalrMqClientService.Subscribe("testapikey", "test");
        }
    }
}