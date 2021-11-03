using Microsoft.Extensions.DependencyInjection;
using SignalrMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class ServiceCollectionsExtensions
{
    // https://csharp.christiannagel.com/2016/07/27/diwithoptions/
    public static IServiceCollection AddSignalrMqClientService(this IServiceCollection services,
        Action<SignalrMqEndpoint> setupAction)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

        services.Configure(setupAction);
        services.AddSingleton<SignalrMqClientService>();

        return services;
    }
}
