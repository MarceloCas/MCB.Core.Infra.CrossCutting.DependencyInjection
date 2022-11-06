using MCB.Core.Infra.CrossCutting.DependencyInjection.Tests.Services.Interfaces;

namespace MCB.Core.Infra.CrossCutting.DependencyInjection.Tests.Services;

public class SingletonService
    : ISingletonService
{
    public Guid Id { get; }

    public SingletonService(IDummyService dummyService)
    {
        Id = Guid.NewGuid();
    }
}
