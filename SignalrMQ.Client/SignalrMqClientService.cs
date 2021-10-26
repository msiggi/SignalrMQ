using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace SignalrMQ.Client
{
    public class SignalrMqClientService
    {
        private readonly ILogger<SignalrMqClientService> logger;
        private HubConnection hubConnection;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public SignalrMqClientService(ILogger<SignalrMqClientService> logger)
        {
            this.logger = logger;
        }

        public async Task StartConnection(string host, int port = 443)
        {
            string url = $"https://{host}:{port}/signalrmqbrokerhub";
            hubConnection = new HubConnectionBuilder()
                            .WithUrl(url)
                            .Build();

            hubConnection.Closed += async (error) =>
            {
                logger.LogError($"Connection to {url} closed! Trying to reconnect...");

                await Task.Delay(new Random().Next(0, 5) * 1000);
                await hubConnection.StartAsync();
            };

            hubConnection.On<object>("rcv", rcv =>
            {
                MessageReceived?.Invoke(this, new MessageReceivedEventArgs
                {
                    Payload = rcv
                });
            });

            await Connect(url);
        }

        private async Task Connect(string url)
        {
            try
            {
                await hubConnection.StartAsync();
                logger.LogInformation($"Connection to {url} established!");
            }
            catch (Exception ex)
            {
                logger.LogError($"No connection to {url} established! Trrying again...");

                await Task.Delay(new Random().Next(0, 5) * 1000);
                await Connect(url);
            }
        }

        public async Task Publish(string exchangename, object payload)
        {
            await hubConnection.SendAsync("Publish", exchangename, payload);
        }
        public async Task Subscribe(string exchangename)
        {
            await hubConnection.SendAsync("Subscribe", exchangename);
        }
        public async Task Unsubscribe(string exchangename)
        {
            await hubConnection.SendAsync("Unsubscribe", exchangename);
        }

    }
}