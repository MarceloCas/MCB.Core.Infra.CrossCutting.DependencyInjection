using MCB.Core.Infra.CrossCutting.DependencyInjection.Tests.Services.Interfaces;

namespace MCB.Core.Infra.CrossCutting.DependencyInjection.Tests.Services;

public class ScopedService
    : IScopedService
{
    public Guid Id { get; }

    public ScopedService(IDummyService dummyService)
    {
        Id = Guid.NewGuid();
    }
}
