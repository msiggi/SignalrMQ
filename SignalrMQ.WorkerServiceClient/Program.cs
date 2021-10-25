using SignalrMQ.Client;
using SignalrMQ.WorkerServiceClient;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<SignalrMqClientService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
