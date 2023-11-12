using Microsoft.Extensions.DependencyInjection;
using MyHome;

namespace Namespace;
public static class ServicesExtensions
{
    public static IServiceCollection AddMyHome(this IServiceCollection services)
    {
        services.AddSingleton<Func<IDynamicLightAdjuster.DynamicLightModel, IDynamicLightAdjuster>>(model => new DynamicLightAdjuster(model));
        return services;
    }
}
