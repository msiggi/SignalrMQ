using SignalrMQ.Client;
using SignalrMQ.WorkerServiceClient;
using SignalrMQ.WorkerServiceClient.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        BrokerSettings brokerConfig = hostContext.Configuration.GetSection(nameof(BrokerSettings)).Get<BrokerSettings>();
        AppSettings.BrokerSettings = brokerConfig;

        services.AddSingleton<SignalrMqClientService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
