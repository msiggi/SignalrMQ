using SignalrMQ.Client;
using SignalrMQ.WorkerServiceClientRcv;
using SignalrMQ.WorkerServiceClientRcv.Configuration;

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
