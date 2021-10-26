using SignalrMQ.Client;
using SignalrMQ.WorkerServiceClientRcv;
using SignalrMQ.WorkerServiceClientRcv.Configuration;

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
