using MCB.Core.Infra.CrossCutting.DependencyInjection.Tests.Services.Interfaces;

namespace MCB.Core.Infra.CrossCutting.DependencyInjection.Tests.Services;

public class TransientService
    : ITransientService
{
    public Guid Id { get; }

    public TransientService(IDummyService dummyService)
    {
        Id = Guid.NewGuid();
    }
}
