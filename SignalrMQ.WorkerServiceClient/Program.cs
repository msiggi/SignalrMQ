using SignalrMQ.Client;
using SignalrMQ.WorkerServiceClient;
using SignalrMQ.WorkerServiceClient.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        SignalrMqBrokerInformation brokerConfig = hostContext.Configuration.GetSection(nameof(SignalrMqBrokerInformation)).Get<SignalrMqBrokerInformation>();
        AppSettings.BrokerSettings = brokerConfig;

        services.AddSingleton<SignalrMqClientService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
