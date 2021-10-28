﻿using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using SignalrMQ.Core;

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

            hubConnection.On<MessageItem>("rcv", rcv =>
            {
                MessageReceived?.Invoke(this, new MessageReceivedEventArgs
                {
                    ReferenceCode = rcv.ReferenceCode,
                    Payload = rcv.Payload
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

        public async Task Publish(string apiKey, string exchangename, string referenceCode, object payload)
        {
            await hubConnection.SendAsync("Publish", apiKey, exchangename, referenceCode, payload);
        }



        public async Task Subscribe(string apiKey, string exchangename)
        {
            await hubConnection.SendAsync("Subscribe", apiKey, exchangename);
        }
        public async Task Unsubscribe(string apiKey, string exchangename)
        {
            await hubConnection.SendAsync("Unsubscribe", apiKey, exchangename);
        }

    }
}