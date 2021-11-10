using SignalrMQ.Client;
using SignalrMQ.Client.Configuration;
using SignalrMQ.WorkerServiceClientRcv;
using SignalrMQ.WorkerServiceClientRcv.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSignalrMqClientService(opts => hostContext.Configuration.GetSection(nameof(SignalrMqEndpoint)).Bind(opts));

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
