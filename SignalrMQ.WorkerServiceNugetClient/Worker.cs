using SignalrMQ.Client;

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
        }

        private void SignalrMqClientService_ConnectionEstablished(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}