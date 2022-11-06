using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MCB.Core.Infra.CrossCutting.DependencyInjection;

public static class DependencyInjectionExtensionMethods
{
    public static void AddMcbDependencyInjection(
        this IServiceCollection services,
        Action<IDependencyInjectionContainer> configureServicesAction,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped
    )
    {
        var dependencyInjectionContainer = new DependencyInjectionContainer(services);
        configureServicesAction(dependencyInjectionContainer);

        services.Add(
            new ServiceDescriptor(
                serviceType: typeof(IDependencyInjectionContainer),
                factory: serviceProvider => dependencyInjectionContainer.Build(),
                lifetime: serviceLifetime
            )
        );
    }
}
