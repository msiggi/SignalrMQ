using Microsoft.AspNetCore.SignalR.Client;
using SignalrMQ.Client;
using SignalrMQ.WorkerServiceClient.Configuration;

namespace SignalrMQ.WorkerServiceClient
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly SignalrMqClientService signalrMqClientService;
        private object connection;

        public Worker(ILogger<Worker> logger, SignalrMqClientService signalrMqClientService)
        {
            _logger = logger;
            this.signalrMqClientService = signalrMqClientService;
            //this.signalrMqClientService.StartConnection(AppSettings.BrokerSettings.Host, AppSettings.BrokerSettings.Port).GetAwaiter().GetResult();
        }

        //public Worker(ILogger<Worker> logger)
        //{
        //    _logger = logger;
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Sende..." + DateTime.Now + " " + i);
                //await signalrMqClientService.Publish("testapikey", "test", "testref", "Payload: " + DateTime.Now + " " + i);
                i++;
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}