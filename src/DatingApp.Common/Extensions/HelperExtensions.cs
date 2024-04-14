using DatingApp.Common.Helpers.User;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DatingApp.Common.Extensions
{
    public static class HelperExtensions
    {
        public static IServiceCollection AddCommonHelpers(this IServiceCollection services)
        {
            services.AddScoped<IUser, CurrentUser>();

            return services;
        }

        public static IServiceCollection AddMediatrFromAssemblyContaining(this IServiceCollection services, Assembly assembly, Dictionary<Type, Type>? pipelineBehaviors = null)
        {
            return services
                        .AddMediatR(cfg =>
                        {
                            cfg.RegisterServicesFromAssembly(assembly);
                            if(pipelineBehaviors != null)
                            {
                                foreach(var pipelineBehavior in pipelineBehaviors)
                                {
                                    cfg.AddBehavior(pipelineBehavior.Key, pipelineBehavior.Value);
                                }
                            }
                        });
        }
    }
}
