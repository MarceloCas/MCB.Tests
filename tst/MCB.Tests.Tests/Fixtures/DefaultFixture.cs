using MCB.Core.Infra.CrossCutting.DependencyInjection;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Tests.Fixtures;
using MCB.Tests.Tests.Services;
using MCB.Tests.Tests.Services.Interfaces;
using Xunit;

namespace MCB.Tests.Tests.Fixtures;

[CollectionDefinition(nameof(DefaultFixture))]
public class DefaultFixtureCollection
    : ICollectionFixture<DefaultFixture>
{

}
public class DefaultFixture
    : FixtureBase
{
    // Protected Methods
    protected override IDependencyInjectionContainer CreateDependencyInjectionContainerInternal()
    {
        return new DependencyInjectionContainer();
    }
    protected override void BuildDependencyInjectionContainerInternal(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        ((DependencyInjectionContainer)dependencyInjectionContainer).Build();
    }
    protected override void ConfigureDependencyInjectionContainerInternal(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        dependencyInjectionContainer.RegisterScoped<IDummyService, DummyService>();
    }

}
