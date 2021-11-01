using SignalrMQ.Client;
using SignalrMQ.WorkerServiceClient;
using SignalrMQ.WorkerServiceClient.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        //SignalrMqBrokerInformation brokerConfig = hostContext.Configuration
        //    .GetSection(nameof(SignalrMqBrokerInformation))
        //    .Get<SignalrMqBrokerInformation>();
        //AppSettings.BrokerSettings = brokerConfig;

        services.AddSignalrMqClientService(opts => hostContext.Configuration.GetSection(nameof(SignalrMqBrokerInformation)).Bind(opts));
        services.AddHostedService<Worker>();

    })
    .Build();

await host.RunAsync();
