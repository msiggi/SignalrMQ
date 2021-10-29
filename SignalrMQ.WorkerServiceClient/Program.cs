using SignalrMQ.Client;
using SignalrMQ.WorkerServiceClient;
using SignalrMQ.WorkerServiceClient.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        SignalrMqBrokerInformation brokerConfig = hostContext.Configuration
            .GetSection(nameof(SignalrMqBrokerInformation))
            .Get<SignalrMqBrokerInformation>();
        AppSettings.BrokerSettings = brokerConfig;

        //services.Configure<SignalrMqBrokerInformation>(hostContext.Configuration.GetSection(nameof(SignalrMqBrokerInformation)));
        //services.AddSingleton<ISignalrMqClientService, SignalrMqClientService>();

        services.AddSignalrMqClientService(opts => opts = brokerConfig);

        services.AddHostedService<Worker>();

    })
    .Build();

await host.RunAsync();
