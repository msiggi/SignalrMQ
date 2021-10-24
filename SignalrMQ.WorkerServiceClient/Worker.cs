using Microsoft.AspNetCore.SignalR.Client;

namespace SignalrMQ.WorkerServiceClient
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private object connection;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            HubConnection connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7141/BrokerHub")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            await connection.StartAsync();

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