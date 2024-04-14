using DatingApp.Common.Settings;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DatingApp.Common.Extensions
{
    public static class RabbitMqExtensions
    {
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration, 
            List<Assembly>? extraEventHandlerAssemblies = null)
        {
            services
                .AddMassTransit(configure =>
                {
                    configure.AddConsumers(Assembly.GetEntryAssembly());

                    foreach (var assembly in extraEventHandlerAssemblies ?? Enumerable.Empty<Assembly>())
                        configure.AddConsumers(assembly);

                    configure.UsingRabbitMq((context, configurator) =>
                    {
                        var configuration = context.GetService<IConfiguration>();
                        var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                        var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
                        configurator.Host(rabbitMQSettings.Host);
                        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
                    });
                })
                .AddMassTransitHostedService();

            return services;
        }
    }
}
