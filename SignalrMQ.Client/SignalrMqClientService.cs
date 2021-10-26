using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace SignalrMQ.Client
{
    public class SignalrMqClientService
    {
        private readonly ILogger<SignalrMqClientService> logger;

        public SignalrMqClientService(ILogger<SignalrMqClientService> logger)
        {
            this.logger = logger;
        }

        public async Task StartConnection(string host, int port = 443)
        {
            HubConnection hubConnection = new HubConnectionBuilder()
                            .WithUrl($"https://{host}:{port}/signalrmqbrokerhub")
                            .Build();

            hubConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };

            await hubConnection.StartAsync();

            await hubConnection.InvokeAsync("Publish", "sdfsd", "sdfsdfr");
        }
    }
}