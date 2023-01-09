using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;

namespace MCB.Tests.Fixtures;

public abstract class FixtureBase
{
    // Properties
    public Guid TenantId { get; }
    public string ExecutionUser { get; }
    public string SourcePlatform { get; }

    // Constructors
    protected FixtureBase()
    {
        TenantId = GenerateNewTenantId();
        ExecutionUser = GenerateNewExecutionUser();
        SourcePlatform = GenerateNewSourcePlatform();
    }

    // Abstract Methods
    protected abstract IDependencyInjectionContainer CreateDependencyInjectionContainerInternal();
    protected abstract void ConfigureDependencyInjectionContainerInternal(IDependencyInjectionContainer dependencyInjectionContainer);
    protected abstract void BuildDependencyInjectionContainerInternal(IDependencyInjectionContainer dependencyInjectionContainer);

    // Public Methods
    public IDependencyInjectionContainer CreateNewDependencyInjectionContainer()
    {
        var dependencyInjectionContainer = CreateDependencyInjectionContainerInternal();
        ConfigureDependencyInjectionContainerInternal(dependencyInjectionContainer);
        BuildDependencyInjectionContainerInternal(dependencyInjectionContainer);

        return dependencyInjectionContainer;
    }

    // Public Statis Methods
    public static Guid GenerateNewTenantId() => Guid.NewGuid();
    public static string GenerateNewExecutionUser() => $"{nameof(ExecutionUser)} {Guid.NewGuid()}";
    public static string GenerateNewSourcePlatform() => $"{nameof(SourcePlatform)} {Guid.NewGuid()}";
    public static Guid GenerateCorrelationId() => Guid.NewGuid();
}
