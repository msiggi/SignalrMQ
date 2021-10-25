using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace SignalrMQ.Client
{
    public class SignalrMqClientService
    {
        public SignalrMqClientService(ILogger<SignalrMqClientService> logger)
        {
            Task.Run(async () =>
            {
                await StartConnection();
            });
        }

        public async Task StartConnection()
        {
            HubConnection hubConnection = new HubConnectionBuilder()
                            .WithUrl("https://localhost:7284/BrokerHub")
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