using Microsoft.AspNetCore.SignalR.Client;
using SignalrMQ.Client;

namespace SignalrMQ.WorkerServiceClient
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private object connection;

        public Worker(ILogger<Worker> logger, SignalrMqClientService signalrMqClientService)
        {
            _logger = logger;
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