using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Enums;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Tests.Services;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Tests.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Xunit;

namespace MCB.Core.Infra.CrossCutting.DependencyInjection.Tests;

public class DependencyInjectionContainerTest
{
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Singleton_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Singleton, typeof(IDummyService), typeof(DummyService));
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Singleton, typeof(ISingletonService), typeof(SingletonService));
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var singletonServiceA = dependencyInjectionContainer.Resolve(typeof(ISingletonService));
        var singletonServiceB = dependencyInjectionContainer.Resolve(typeof(ISingletonService));

        // Assert
        Assert.Equal(singletonServiceA, singletonServiceB);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Transient_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Transient, typeof(IDummyService), typeof(DummyService));
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Transient, typeof(ITransientService), typeof(TransientService));
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var transientServiceA = (ITransientService)dependencyInjectionContainer.Resolve(typeof(ITransientService));
        var transientServiceB = (ITransientService)dependencyInjectionContainer.Resolve(typeof(ITransientService));

        // Assert
        Assert.NotEqual(transientServiceA, transientServiceB);
        Assert.NotEqual(transientServiceA.Id, transientServiceB.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Scoped_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Scoped, typeof(IDummyService), typeof(DummyService));
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Scoped, typeof(IScopedService), typeof(ScopedService));
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var scopedServiceA = (IScopedService)dependencyInjectionContainer.Resolve(typeof(IScopedService));
        var scopedServiceB = (IScopedService)dependencyInjectionContainer.Resolve(typeof(IScopedService));
        dependencyInjectionContainer.CreateNewScope();
        var scopedServiceC = (IScopedService)dependencyInjectionContainer.Resolve(typeof(IScopedService));

        // Assert
        Assert.Equal(scopedServiceA, scopedServiceB);
        Assert.NotEqual(scopedServiceA, scopedServiceC);
        Assert.NotEqual(scopedServiceA.Id, scopedServiceC.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Singleton_Concrete_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Singleton, typeof(IDummyService), typeof(DummyService));
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Singleton, typeof(ConcreteService));
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var concreteServiceA = dependencyInjectionContainer.Resolve(typeof(ConcreteService));
        var concreteServiceB = dependencyInjectionContainer.Resolve(typeof(ConcreteService));

        // Assert
        Assert.Equal(concreteServiceA, concreteServiceB);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Transient_Concrete_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Transient, typeof(IDummyService), typeof(DummyService));
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Transient, typeof(ConcreteService));
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var concreteServiceA = (ConcreteService)dependencyInjectionContainer.Resolve(typeof(ConcreteService));
        var concreteServiceB = (ConcreteService)dependencyInjectionContainer.Resolve(typeof(ConcreteService));

        // Assert
        Assert.NotEqual(concreteServiceA, concreteServiceB);
        Assert.NotEqual(concreteServiceA.Id, concreteServiceB.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Scoped_Concrete_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Scoped, typeof(IDummyService), typeof(DummyService));
                dependencyInjectionContainer.Register(DependencyInjectionLifecycle.Scoped, typeof(ConcreteService));
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var concreteServiceA = (ConcreteService)dependencyInjectionContainer.Resolve(typeof(ConcreteService));
        var concreteServiceB = (ConcreteService)dependencyInjectionContainer.Resolve(typeof(ConcreteService));
        dependencyInjectionContainer.CreateNewScope();
        var concreteServiceC = (ConcreteService)dependencyInjectionContainer.Resolve(typeof(ConcreteService));

        // Assert
        Assert.Equal(concreteServiceA, concreteServiceB);
        Assert.NotEqual(concreteServiceA, concreteServiceC);
        Assert.NotEqual(concreteServiceA.Id, concreteServiceC.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Unregister_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterSingleton<IUnregisteredService, UnregisteredService>();
                dependencyInjectionContainer.Unregister(typeof(IUnregisteredService));
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var unregisteredService = dependencyInjectionContainer.Resolve(typeof(IUnregisteredService));

        // Assert
        Assert.Null(unregisteredService);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Unregister_When_Register_Not_Exists_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.Unregister(typeof(IUnregisteredService));
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var unregisteredService = dependencyInjectionContainer.Resolve(typeof(IUnregisteredService));

        // Assert
        Assert.Null(unregisteredService);
    }

    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Singleton_Services_With_Generic()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterSingleton<ISingletonService, SingletonService>();
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var singletonServiceA = dependencyInjectionContainer.Resolve<ISingletonService>();
        var singletonServiceB = dependencyInjectionContainer.Resolve<ISingletonService>();

        // Assert
        Assert.Equal(singletonServiceA, singletonServiceB);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Transient_Services_With_Generic()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterTransient<ITransientService, TransientService>();
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var transientServiceA = dependencyInjectionContainer.Resolve<ITransientService>();
        var transientServiceB = dependencyInjectionContainer.Resolve<ITransientService>();

        // Assert
        Assert.NotEqual(transientServiceA, transientServiceB);
        Assert.NotEqual(transientServiceA.Id, transientServiceB.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Scoped_Services_With_Generic()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterScoped<IScopedService, ScopedService>();
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var scopedServiceA = dependencyInjectionContainer.Resolve<IScopedService>();
        var scopedServiceB = dependencyInjectionContainer.Resolve<IScopedService>();
        dependencyInjectionContainer.CreateNewScope();
        var scopedServiceC = dependencyInjectionContainer.Resolve<IScopedService>();

        // Assert
        Assert.Equal(scopedServiceA, scopedServiceB);
        Assert.NotEqual(scopedServiceA, scopedServiceC);
        Assert.NotEqual(scopedServiceA.Id, scopedServiceC.Id);
    }

    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Singleton_Services_With_Generic_And_Factory()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterSingleton<ISingletonService, SingletonService>(
                    dependencyInjectionContainer => new SingletonService(dependencyInjectionContainer.Resolve<IDummyService>())
                );
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var singletonServiceA = dependencyInjectionContainer.Resolve<ISingletonService>();
        var singletonServiceB = dependencyInjectionContainer.Resolve<ISingletonService>();

        // Assert
        Assert.Equal(singletonServiceA, singletonServiceB);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Transient_Services_With_Generic_And_Factory()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterTransient<ITransientService, TransientService>(
                    dependencyInjectionContainer => new TransientService(dependencyInjectionContainer.Resolve<IDummyService>())
                );
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var transientServiceA = dependencyInjectionContainer.Resolve<ITransientService>();
        var transientServiceB = dependencyInjectionContainer.Resolve<ITransientService>();

        // Assert
        Assert.NotEqual(transientServiceA, transientServiceB);
        Assert.NotEqual(transientServiceA.Id, transientServiceB.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Scoped_Services_With_Generic_And_Factory()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterScoped<IScopedService, ScopedService>(
                    dependencyInjectionContainer => new ScopedService(dependencyInjectionContainer.Resolve<IDummyService>())
                );
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var scopedServiceA = dependencyInjectionContainer.Resolve<IScopedService>();
        var scopedServiceB = dependencyInjectionContainer.Resolve<IScopedService>();
        dependencyInjectionContainer.CreateNewScope();
        var scopedServiceC = dependencyInjectionContainer.Resolve<IScopedService>();

        // Assert
        Assert.Equal(scopedServiceA, scopedServiceB);
        Assert.NotEqual(scopedServiceA, scopedServiceC);
        Assert.NotEqual(scopedServiceA.Id, scopedServiceC.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Singleton_Concrete_Services_With_Generic()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterSingleton<ConcreteService>();
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var singletonServiceA = dependencyInjectionContainer.Resolve<ConcreteService>();
        var singletonServiceB = dependencyInjectionContainer.Resolve<ConcreteService>();

        // Assert
        Assert.Equal(singletonServiceA, singletonServiceB);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Transient_Concrete_Services_With_Generic()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterTransient<ConcreteService>();
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var concreteServiceA = dependencyInjectionContainer.Resolve<ConcreteService>();
        var concreteServiceB = dependencyInjectionContainer.Resolve<ConcreteService>();

        // Assert
        Assert.NotEqual(concreteServiceA, concreteServiceB);
        Assert.NotEqual(concreteServiceA.Id, concreteServiceB.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Scoped_Concrete_Services_With_Generic()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterScoped<ConcreteService>();
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var concreteServiceA = dependencyInjectionContainer.Resolve<ConcreteService>();
        var concreteServiceB = dependencyInjectionContainer.Resolve<ConcreteService>();
        dependencyInjectionContainer.CreateNewScope();
        var concreteServiceC = dependencyInjectionContainer.Resolve<ConcreteService>();

        // Assert
        Assert.Equal(concreteServiceA, concreteServiceB);
        Assert.NotEqual(concreteServiceA, concreteServiceC);
        Assert.NotEqual(concreteServiceA.Id, concreteServiceC.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Unregister_Services_With_Generic()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterSingleton<IUnregisteredService, UnregisteredService>();
                dependencyInjectionContainer.Unregister<IUnregisteredService>();
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var unregisteredService = dependencyInjectionContainer.Resolve<IUnregisteredService>();

        // Assert
        Assert.Null(unregisteredService);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Unregister_Services_When_Register_Not_Exists_With_Generic()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.Unregister<IUnregisteredService>();
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var unregisteredService = dependencyInjectionContainer.Resolve<IUnregisteredService>();

        // Assert
        Assert.Null(unregisteredService);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Not_ConvertToDependencyInjectionLifecycle()
    {
        // Arrange
        var depencyInjectionContainer = new DependencyInjectionContainer(null);
        var hasThrowException = false;

        var methodInfo = typeof(DependencyInjectionContainer).GetMethod("ConvertToDependencyInjectionLifecycle", BindingFlags.NonPublic | BindingFlags.Static);
        object[] parameters = { -1 };

        // Act
        try
        {
            methodInfo.Invoke(depencyInjectionContainer, parameters);
        }
        catch (TargetInvocationException ex)
        {
            if(ex.InnerException is ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                hasThrowException = argumentOutOfRangeException.ParamName == "lifecycle";
            }
        }

        // Assert
        Assert.True(hasThrowException);
    }

    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Singleton_Services_With_Factory()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterSingleton<ISingletonService>(dependencyInjectionContainer =>
                {
                    return new SingletonService(dependencyInjectionContainer.Resolve<IDummyService>());
                });
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var singletonServiceA = dependencyInjectionContainer.Resolve<ISingletonService>();
        var singletonServiceB = dependencyInjectionContainer.Resolve<ISingletonService>();

        // Assert
        Assert.Equal(singletonServiceA, singletonServiceB);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Transient_Services_With_Factory()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterTransient<ITransientService>(dependencyInjectionContainer =>
                {
                    return new TransientService(dependencyInjectionContainer.Resolve<IDummyService>());
                });
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var transientServiceA = dependencyInjectionContainer.Resolve<ITransientService>();
        var transientServiceB = dependencyInjectionContainer.Resolve<ITransientService>();

        // Assert
        Assert.NotEqual(transientServiceA, transientServiceB);
        Assert.NotEqual(transientServiceA.Id, transientServiceB.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Scoped_Services_With_Factory()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterScoped<IScopedService>(dependencyInjectionContainer =>
                {
                    return new ScopedService(dependencyInjectionContainer.Resolve<IDummyService>());
                });
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var scopedServiceA = dependencyInjectionContainer.Resolve<IScopedService>();
        var scopedServiceB = dependencyInjectionContainer.Resolve<IScopedService>();
        dependencyInjectionContainer.CreateNewScope();
        var scopedServiceC = dependencyInjectionContainer.Resolve<IScopedService>();

        // Assert
        Assert.Equal(scopedServiceA, scopedServiceB);
        Assert.NotEqual(scopedServiceA, scopedServiceC);
        Assert.NotEqual(scopedServiceA.Id, scopedServiceC.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Singleton_Concrete_Services_With_Factory()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.Register(
                    DependencyInjectionLifecycle.Singleton,
                    serviceType: typeof(ConcreteService),
                    serviceTypeFactory: dependencyInjectionContainer =>
                    {
                        return new ConcreteService(dependencyInjectionContainer.Resolve<IDummyService>());
                    }
                );
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var concreteServiceA = dependencyInjectionContainer.Resolve<ConcreteService>();
        var concreteServiceB = dependencyInjectionContainer.Resolve<ConcreteService>();

        // Assert
        Assert.Equal(concreteServiceA, concreteServiceB);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Singleton_Concrete_Services_With_Factory_Using_Generic()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterSingleton<ConcreteService>(
                    serviceTypeFactory: dependencyInjectionContainer =>
                    {
                        return new ConcreteService(dependencyInjectionContainer.Resolve<IDummyService>());
                    }
                );
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var concreteServiceA = dependencyInjectionContainer.Resolve<ConcreteService>();
        var concreteServiceB = dependencyInjectionContainer.Resolve<ConcreteService>();

        // Assert
        Assert.Equal(concreteServiceA, concreteServiceB);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Transient_Concrete_Services_With_Factory()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.Register(
                    DependencyInjectionLifecycle.Transient,
                    serviceType: typeof(ConcreteService),
                    dependencyInjectionContainer =>
                    {
                        return new ConcreteService(dependencyInjectionContainer.Resolve<IDummyService>());
                    }
                );
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var concreteServiceA = dependencyInjectionContainer.Resolve<ConcreteService>();
        var concreteServiceB = dependencyInjectionContainer.Resolve<ConcreteService>();

        // Assert
        Assert.NotEqual(concreteServiceA, concreteServiceB);
        Assert.NotEqual(concreteServiceA.Id, concreteServiceB.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Transient_Concrete_Services_With_Factory_Using_Generic()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterTransient(dependencyInjectionContainer =>
                {
                    return new ConcreteService(dependencyInjectionContainer.Resolve<IDummyService>());
                });
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var concreteServiceA = dependencyInjectionContainer.Resolve<ConcreteService>();
        var concreteServiceB = dependencyInjectionContainer.Resolve<ConcreteService>();

        // Assert
        Assert.NotEqual(concreteServiceA, concreteServiceB);
        Assert.NotEqual(concreteServiceA.Id, concreteServiceB.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Scoped_Concrete_Services_With_Factory()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.Register(
                    DependencyInjectionLifecycle.Scoped,
                    serviceType: typeof(ConcreteService),
                    dependencyInjectionContainer =>
                    {
                        return new ConcreteService(dependencyInjectionContainer.Resolve<IDummyService>());
                    }
                );
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var concreteServiceA = dependencyInjectionContainer.Resolve<ConcreteService>();
        var concreteServiceB = dependencyInjectionContainer.Resolve<ConcreteService>();
        dependencyInjectionContainer.CreateNewScope();
        var concreteServiceC = dependencyInjectionContainer.Resolve<ConcreteService>();

        // Assert
        Assert.Equal(concreteServiceA, concreteServiceB);
        Assert.NotEqual(concreteServiceA, concreteServiceC);
        Assert.NotEqual(concreteServiceA.Id, concreteServiceC.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_Scoped_Concrete_Services_With_Factory_Using_Generic()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddMcbDependencyInjection(
            configureServicesAction: dependencyInjectionContainer =>
            {
                dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
                dependencyInjectionContainer.RegisterScoped(dependencyInjectionContainer =>
                {
                    return new ConcreteService(dependencyInjectionContainer.Resolve<IDummyService>());
                });
            }
        );
        var serviceProvider = services.BuildServiceProvider();
        var dependencyInjectionContainer = serviceProvider.GetService<IDependencyInjectionContainer>();

        // Act
        var concreteServiceA = dependencyInjectionContainer.Resolve<ConcreteService>();
        var concreteServiceB = dependencyInjectionContainer.Resolve<ConcreteService>();
        dependencyInjectionContainer.CreateNewScope();
        var concreteServiceC = dependencyInjectionContainer.Resolve<ConcreteService>();

        // Assert
        Assert.Equal(concreteServiceA, concreteServiceB);
        Assert.NotEqual(concreteServiceA, concreteServiceC);
        Assert.NotEqual(concreteServiceA.Id, concreteServiceC.Id);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_With_Factory_And_Without_Generic()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        var dependencyInjectionContainer = new DependencyInjectionContainer(serviceCollection);
        dependencyInjectionContainer.Register(
            lifecycle: DependencyInjectionLifecycle.Singleton, 
            serviceType: typeof(IDummyService),
            concreteType: typeof(DummyService), 
            concreteTypeFactory: dependencyInjectionContainer => new InheritedDummyService()
        );
        dependencyInjectionContainer.Build();

        // Act
        var dummyService = dependencyInjectionContainer.Resolve<IDummyService>();

        // Assert
        Assert.True(dummyService is InheritedDummyService);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Resolve_With_Factory_And_Generic()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        var dependencyInjectionContainer = new DependencyInjectionContainer(serviceCollection);
        dependencyInjectionContainer.Register<IDummyService, DummyService>(
            lifecycle: DependencyInjectionLifecycle.Singleton,
            concreteTypeFactory: dependencyInjectionContainer => new InheritedDummyService()
        );
        dependencyInjectionContainer.Build();

        // Act
        var dummyService = dependencyInjectionContainer.Resolve<IDummyService>();

        // Assert
        Assert.True(dummyService is InheritedDummyService);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Get_RegistrationCollection()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        var dependencyInjectionContainer = new DependencyInjectionContainer(serviceCollection);
        dependencyInjectionContainer.RegisterSingleton<ISingletonService, SingletonService>();
        dependencyInjectionContainer.RegisterTransient<ITransientService, TransientService>();
        dependencyInjectionContainer.RegisterScoped<IScopedService, ScopedService>();
        dependencyInjectionContainer.Build();

        // Act
        var registrationCollection = dependencyInjectionContainer.GetRegistrationCollection().ToList();

        // Assert
        Assert.True(registrationCollection.Count == 3);
        Assert.Contains(registrationCollection, q => q.DependencyInjectionLifecycle == DependencyInjectionLifecycle.Singleton && q.ServiceType == typeof(ISingletonService) && q.ConcreteType == typeof(SingletonService));
        Assert.Contains(registrationCollection, q => q.DependencyInjectionLifecycle == DependencyInjectionLifecycle.Transient && q.ServiceType == typeof(ITransientService) && q.ConcreteType == typeof(TransientService));
        Assert.Contains(registrationCollection, q => q.DependencyInjectionLifecycle == DependencyInjectionLifecycle.Scoped && q.ServiceType == typeof(IScopedService) && q.ConcreteType == typeof(ScopedService));
    }

    [Fact]
    public void DependencyInjectionContainer_Should_Not_Resolve_ConcreteType_With_Factory_Return_Null_Value()
    {
        // Arrange
        var expectedExceptionMessage = DependencyInjectionContainer.DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL;
        var raisedExceptionMessage = string.Empty;

        var dependencyInjectionContainer = new DependencyInjectionContainer();

        dependencyInjectionContainer.Register(
            lifecycle: DependencyInjectionLifecycle.Singleton,
            serviceType: typeof(InheritedDummyService),
            serviceTypeFactory: dependencyInjectionContainer => null
        );

        dependencyInjectionContainer.Build();

        // Act
        try
        {
            dependencyInjectionContainer.Resolve<InheritedDummyService>();
        }
        catch (InvalidOperationException ex)
        {
            raisedExceptionMessage = ex.Message;
        }

        // Assert
        Assert.Equal(expectedExceptionMessage, raisedExceptionMessage);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Not_Resolve_ConcreteType_With_Factory_Return_Null_Value_With_Generic()
    {
        // Arrange
        var expectedExceptionMessage = DependencyInjectionContainer.DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL;
        var raisedExceptionMessage = string.Empty;

        var dependencyInjectionContainer = new DependencyInjectionContainer();

        dependencyInjectionContainer.Register<InheritedDummyService>(
            lifecycle: DependencyInjectionLifecycle.Singleton,
            serviceTypeFactory: dependencyInjectionContainer => null
        );

        dependencyInjectionContainer.Build();

        // Act
        try
        {
            dependencyInjectionContainer.Resolve<InheritedDummyService>();
        }
        catch (InvalidOperationException ex)
        {
            raisedExceptionMessage = ex.Message;
        }

        // Assert
        Assert.Equal(expectedExceptionMessage, raisedExceptionMessage);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Not_Resolve_With_Factory_Return_Null_Value_With_Generic()
    {
        // Arrange
        var expectedExceptionMessage = DependencyInjectionContainer.DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL;
        var raisedExceptionMessage = string.Empty;

        var dependencyInjectionContainer = new DependencyInjectionContainer();

        dependencyInjectionContainer.Register<IDummyService, InheritedDummyService>(
            lifecycle: DependencyInjectionLifecycle.Singleton,
            concreteTypeFactory: dependencyInjectionContainer => null
        );

        dependencyInjectionContainer.Build();

        // Act
        try
        {
            dependencyInjectionContainer.Resolve<IDummyService>();
        }
        catch (InvalidOperationException ex)
        {
            raisedExceptionMessage = ex.Message;
        }

        // Assert
        Assert.Equal(expectedExceptionMessage, raisedExceptionMessage);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Not_Resolve_With_Factory_Return_Null_Value()
    {
        // Arrange
        var expectedExceptionMessage = DependencyInjectionContainer.DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL;
        var raisedExceptionMessage = string.Empty;

        var dependencyInjectionContainer = new DependencyInjectionContainer();

        dependencyInjectionContainer.Register(
            lifecycle: DependencyInjectionLifecycle.Singleton,
            serviceType: typeof(IDummyService),
            concreteType: typeof(InheritedDummyService),
            concreteTypeFactory: dependencyInjectionContainer => null
        );

        dependencyInjectionContainer.Build();

        // Act
        try
        {
            dependencyInjectionContainer.Resolve<IDummyService>();
        }
        catch (InvalidOperationException ex)
        {
            raisedExceptionMessage = ex.Message;
        }

        // Assert
        Assert.Equal(expectedExceptionMessage, raisedExceptionMessage);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Not_Resolve_ConcreteType_Without_Generic_And_Null_ConcreteFactory()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        var dependencyInjectionContainer = new DependencyInjectionContainer(serviceCollection);
        dependencyInjectionContainer.Register(lifecycle: DependencyInjectionLifecycle.Singleton, serviceType: typeof(DummyService), serviceTypeFactory: null);

        var expectedExceptionMessage = DependencyInjectionContainer.DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL;
        var raisedExceptionMessage = string.Empty;

        // Act
        try
        {
            dependencyInjectionContainer.Resolve<DummyService>();
        }
        catch (InvalidOperationException ex)
        {
            raisedExceptionMessage = ex.Message;
        }

        // Assert
        Assert.Equal(expectedExceptionMessage, raisedExceptionMessage);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Not_Resolve_ConcreteType_Without_Generic_And_ConcreteFactory_Returning_Null()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();

        var dependencyInjectionContainer = new DependencyInjectionContainer(serviceCollection);
        dependencyInjectionContainer.Register(
            lifecycle: DependencyInjectionLifecycle.Singleton, 
            serviceType: typeof(DummyService), 
            serviceTypeFactory: dependencyInjectionContainer => null
        );

        var expectedExceptionMessage = DependencyInjectionContainer.DEPENDENCY_INJECTION_CONTAINER_OBJECT_CANNOT_BE_NULL;
        var raisedExceptionMessage = string.Empty;

        // Act
        try
        {
            dependencyInjectionContainer.Resolve<DummyService>();
        }
        catch (InvalidOperationException ex)
        {
            raisedExceptionMessage = ex.Message;
        }

        // Assert
        Assert.Equal(expectedExceptionMessage, raisedExceptionMessage);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Not_CreateNewScope()
    {
        // Arrange
        var dependencyInjectionContainer = new DependencyInjectionContainer();
        var raisedExceptionMessage = string.Empty;
        var expectedExceptionMessage = DependencyInjectionContainer.DEPENDENCY_INJECTION_CONTAINER_SHOULD_BUILD;

        // Act
        try
        {
            dependencyInjectionContainer.CreateNewScope();
        }
        catch (Exception ex)
        {
            raisedExceptionMessage = ex.Message;
        }

        // Assert
        Assert.Equal(expectedExceptionMessage, raisedExceptionMessage);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Not_Resolve()
    {
        // Arrange
        var dependencyInjectionContainer = new DependencyInjectionContainer();
        var raisedExceptionMessage = string.Empty;
        var expectedExceptionMessage = DependencyInjectionContainer.DEPENDENCY_INJECTION_CONTAINER_SHOULD_BUILD;

        // Act
        try
        {
            var dummyService = dependencyInjectionContainer.Resolve(typeof(IDummyService));
        }
        catch (InvalidOperationException ex)
        {
            raisedExceptionMessage = ex.Message;
        }

        // Assert
        Assert.Equal(expectedExceptionMessage, raisedExceptionMessage);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Not_Resolve_With_Generic()
    {
        // Arrange
        var dependencyInjectionContainer = new DependencyInjectionContainer();
        var raisedExceptionMessage = string.Empty;
        var expectedExceptionMessage = DependencyInjectionContainer.DEPENDENCY_INJECTION_CONTAINER_SHOULD_BUILD;

        // Act
        try
        {
            var dummyService = dependencyInjectionContainer.Resolve<IDummyService>();
        }
        catch (InvalidOperationException ex)
        {
            raisedExceptionMessage = ex.Message;
        }

        // Assert
        Assert.Equal(expectedExceptionMessage, raisedExceptionMessage);
    }
    [Fact]
    public void DependencyInjectionContainer_Should_Not_Register_With_Invalid_Lifecycle()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var dependencyInjectionContainer = new DependencyInjectionContainer(serviceCollection);
        var raisedException = false;

        // Act
        try
        {
            dependencyInjectionContainer.Register<IDummyService>(lifecycle: 0);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            raisedException = ex.ParamName == "lifecycle";
        }

        // Assert
        Assert.True(raisedException);
    }
}
