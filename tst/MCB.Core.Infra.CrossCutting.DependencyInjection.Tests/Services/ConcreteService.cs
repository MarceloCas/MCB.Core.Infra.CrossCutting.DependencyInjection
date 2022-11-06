using MCB.Core.Infra.CrossCutting.DependencyInjection.Tests.Services.Interfaces;

namespace MCB.Core.Infra.CrossCutting.DependencyInjection.Tests.Services;

public class ConcreteService
{
    public Guid Id { get; }

    public ConcreteService(IDummyService dummyService)
    {
        Id = Guid.NewGuid();
    }
}
