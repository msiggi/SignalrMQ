using Microsoft.Extensions.DependencyInjection;
using SignalrMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


static class ServiceCollectionsExtensions
{
    // https://csharp.christiannagel.com/2016/07/27/diwithoptions/
    public static IServiceCollection AddSignalrMqClientService(this IServiceCollection collection,
        Action<SignalrMqBrokerInformation> setupAction)
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

        collection.Configure(setupAction);
        return collection.AddTransient<ISignalrMqClientService, SignalrMqClientService>();
    }
}
