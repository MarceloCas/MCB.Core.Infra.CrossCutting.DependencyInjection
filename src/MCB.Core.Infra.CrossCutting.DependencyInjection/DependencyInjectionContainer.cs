using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Enums;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MCB.Core.Infra.CrossCutting.DependencyInjection;

public class DependencyInjectionContainer
    : IDependencyInjectionContainer
{
    // Constants
    public const string DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL = nameof(DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL);
    public const string DEPENDENCY_INJECTION_CONTAINER_SHOULD_BUILD = nameof(DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL);

    // Fields
    private readonly IServiceCollection _serviceCollection;
    private IServiceProvider? _rootServiceProvider;
    private IServiceProvider? _currentServiceProvider;

    // Constructors
    public DependencyInjectionContainer(IServiceCollection? existingServiceCollection = null)
    {
        _serviceCollection = existingServiceCollection ?? new ServiceCollection();
    }

    // Public Methods
    public DependencyInjectionContainer Build(IServiceProvider? serviceProvider = null)
    {
        _rootServiceProvider = serviceProvider ?? _serviceCollection.BuildServiceProvider();
        _currentServiceProvider = _rootServiceProvider;
        return this;
    }

    #region [ Scopes ]
    public void CreateNewScope()
    {
        if (_rootServiceProvider is null)
            throw new InvalidOperationException(DEPENDENCY_INJECTION_CONTAINER_SHOULD_BUILD);

        _currentServiceProvider = _rootServiceProvider.CreateScope().ServiceProvider;
    }
    #endregion

    #region [ Resolve ]
    public object? Resolve(Type type)
    {
        if (_currentServiceProvider is null)
            throw new InvalidOperationException(DEPENDENCY_INJECTION_CONTAINER_SHOULD_BUILD);

        return _currentServiceProvider.GetService(type);
    }
    public TType? Resolve<TType>()
    {
        if (_currentServiceProvider is null)
            throw new InvalidOperationException(DEPENDENCY_INJECTION_CONTAINER_SHOULD_BUILD);

        return _currentServiceProvider.GetService<TType>();
    }
    #endregion

    #region [ Register ]
    private static ServiceLifetime ConvertToServiceLifetime(DependencyInjectionLifecycle lifecycle)
    {
        return lifecycle switch
        {
            DependencyInjectionLifecycle.Transient => ServiceLifetime.Transient,
            DependencyInjectionLifecycle.Scoped => ServiceLifetime.Scoped,
            DependencyInjectionLifecycle.Singleton => ServiceLifetime.Singleton,
            _ => throw new ArgumentOutOfRangeException(nameof(lifecycle)),
        };
    }
    private static DependencyInjectionLifecycle ConvertToDependencyInjectionLifecycle(ServiceLifetime lifecycle)
    {
        return lifecycle switch
        {
            ServiceLifetime.Transient => DependencyInjectionLifecycle.Transient,
            ServiceLifetime.Scoped => DependencyInjectionLifecycle.Scoped,
            ServiceLifetime.Singleton => DependencyInjectionLifecycle.Singleton,
            _ => throw new ArgumentOutOfRangeException(nameof(lifecycle)),
        };
    }

    public void Register(DependencyInjectionLifecycle lifecycle, Type serviceType)
    {
        _serviceCollection.Add(
            new ServiceDescriptor(
                serviceType: serviceType,
                implementationType: serviceType,
                lifetime: ConvertToServiceLifetime(lifecycle)
            )
        );
    }
    public void Register(DependencyInjectionLifecycle lifecycle, Type serviceType, Func<IDependencyInjectionContainer, object?> serviceTypeFactory)
    {
        _serviceCollection.Add(
            new ServiceDescriptor(
                serviceType: serviceType,
                factory: serviceProvider =>
                {
                    var concreteObject = serviceTypeFactory(this);

                    if (concreteObject is null)
                        throw new InvalidOperationException(DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL);

                    return concreteObject;
                },
                lifetime: ConvertToServiceLifetime(lifecycle)
            )
        );
    }
    public void Register(DependencyInjectionLifecycle lifecycle, Type serviceType, Type concreteType)
    {
        _serviceCollection.Add(
            new ServiceDescriptor(
                serviceType: serviceType,
                implementationType: concreteType,
                lifetime: ConvertToServiceLifetime(lifecycle)
            )
        );
    }
    public void Register(DependencyInjectionLifecycle lifecycle, Type serviceType, Type concreteType, Func<IDependencyInjectionContainer, object?> concreteTypeFactory)
    {
        _serviceCollection.Add(
            new ServiceDescriptor(
                serviceType: serviceType,
                factory: serviceProvider =>
                {
                    var concreteObject = concreteTypeFactory(this);

                    if (concreteObject is null)
                        throw new InvalidOperationException(DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL);

                    return concreteObject;
                },
                lifetime: ConvertToServiceLifetime(lifecycle)
            )
        );
    }

    public void Register<TConcreteType>(DependencyInjectionLifecycle lifecycle)
    {
        Register(lifecycle, typeof(TConcreteType));
    }
    public void Register<TConcreteType>(DependencyInjectionLifecycle lifecycle, Func<IDependencyInjectionContainer, TConcreteType?> serviceTypeFactory)
    {
        _serviceCollection.Add(
            new ServiceDescriptor(
                serviceType: typeof(TConcreteType),
                factory: serviceProvider =>
                {
                    var concreteObject = serviceTypeFactory(this);

                    if (concreteObject is null)
                        throw new InvalidOperationException(DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL);

                    return concreteObject;
                },
                lifetime: ConvertToServiceLifetime(lifecycle)
            )
        );
    }
    public void Register<TAbstractionType, TConcreteType>(DependencyInjectionLifecycle lifecycle)
    {
        _serviceCollection.Add(
            new ServiceDescriptor(
                serviceType: typeof(TAbstractionType),
                implementationType: typeof(TConcreteType),
                lifetime: ConvertToServiceLifetime(lifecycle)
            )
        );
    }
    public void Register<TAbstractionType, TConcreteType>(DependencyInjectionLifecycle lifecycle, Func<IDependencyInjectionContainer, TConcreteType?> concreteTypeFactory)
    {
        _serviceCollection.Add(
            new ServiceDescriptor(
                serviceType: typeof(TAbstractionType),
                factory: serviceProvider =>
                {
                    var concreteObject = concreteTypeFactory(this);

                    if (concreteObject is null)
                        throw new InvalidOperationException(DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL);

                    return concreteObject;
                },
                lifetime: ConvertToServiceLifetime(lifecycle)
            )
        );
    }

    public void RegisterScoped<TConcreteType>()
    {
        Register<TConcreteType>(DependencyInjectionLifecycle.Scoped);
    }
    public void RegisterScoped<TConcreteType>(Func<IDependencyInjectionContainer, TConcreteType?> serviceTypeFactory)
    {
        Register(DependencyInjectionLifecycle.Scoped, serviceTypeFactory);
    }
    public void RegisterScoped<TAbstractionType, TConcreteType>()
    {
        Register<TAbstractionType, TConcreteType>(DependencyInjectionLifecycle.Scoped);
    }
    public void RegisterScoped<TAbstractionType, TConcreteType>(Func<IDependencyInjectionContainer, TConcreteType?> concreteTypeFactory)
    {
        Register<TAbstractionType, TConcreteType>(DependencyInjectionLifecycle.Scoped, concreteTypeFactory);
    }

    public void RegisterSingleton<TConcreteType>()
    {
        Register<TConcreteType>(DependencyInjectionLifecycle.Singleton);
    }
    public void RegisterSingleton<TConcreteType>(Func<IDependencyInjectionContainer, TConcreteType?> serviceTypeFactory)
    {
        Register(DependencyInjectionLifecycle.Singleton, serviceTypeFactory);
    }
    public void RegisterSingleton<TAbstractionType, TConcreteType>()
    {
        Register<TAbstractionType, TConcreteType>(DependencyInjectionLifecycle.Singleton);
    }
    public void RegisterSingleton<TAbstractionType, TConcreteType>(Func<IDependencyInjectionContainer, TConcreteType?> concreteTypeFactory)
    {
        Register<TAbstractionType, TConcreteType>(DependencyInjectionLifecycle.Singleton, concreteTypeFactory);
    }

    public void RegisterTransient<TConcreteType>()
    {
        Register<TConcreteType>(DependencyInjectionLifecycle.Transient);
    }
    public void RegisterTransient<TConcreteType>(Func<IDependencyInjectionContainer, TConcreteType?> serviceTypeFactory)
    {
        Register(DependencyInjectionLifecycle.Transient, serviceTypeFactory);
    }
    public void RegisterTransient<TAbstractionType, TConcreteType>()
    {
        Register<TAbstractionType, TConcreteType>(DependencyInjectionLifecycle.Transient);
    }
    public void RegisterTransient<TAbstractionType, TConcreteType>(Func<IDependencyInjectionContainer, TConcreteType?> concreteTypeFactory)
    {
        Register<TAbstractionType, TConcreteType>(DependencyInjectionLifecycle.Transient, concreteTypeFactory);
    }
    #endregion

    #region [ Unregister ]
    public void Unregister(Type serviceType)
    {
        var serviceDescriptor = _serviceCollection.FirstOrDefault(descriptor => descriptor.ServiceType == serviceType);

        if (serviceDescriptor != null)
            _serviceCollection.Remove(serviceDescriptor);
    }
    public void Unregister<T>()
    {
        Unregister(typeof(T));
    }
    #endregion

    public IEnumerable<Registration> GetRegistrationCollection()
    {
        foreach (var service in _serviceCollection)
            yield return new Registration(
                serviceType: service.ServiceType,
                concreteType: service.ImplementationType,
                dependencyInjectionLifecycle: ConvertToDependencyInjectionLifecycle(service.Lifetime)
            );
    }
}
