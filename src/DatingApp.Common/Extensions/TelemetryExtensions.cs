using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace DatingApp.Common.Extensions;

public static class TelemetryExtensions
{
    public static IServiceCollection AddTelemetryExtensions(this IServiceCollection services, string serviceName)
    {
        services
            .AddOpenTelemetry()
            .WithTracing(b =>
            {
                b
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddHttpClientInstrumentation()
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                    .AddSource(serviceName)
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        options.SetDbStatementForText = true;
                        options.SetDbStatementForStoredProcedure = true;
                        options.EnrichWithIDbCommand = (activity, command) =>
                        {
                            var stateDisplayName = $"{command.CommandType} main";
                            activity.DisplayName = stateDisplayName;
                            activity.SetTag("db.name", stateDisplayName);
                        };
                    })
                    .AddOtlpExporter();
            });

        return services;
    }
}