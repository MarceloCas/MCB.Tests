using FluentAssertions;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using MCB.Tests.Fixtures;
using MCB.Tests.Tests.DomainEntities;
using MCB.Tests.Tests.Fixtures;
using MCB.Tests.Tests.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MCB.Tests.Tests;

[Collection(nameof(DefaultFixture))]
public class DummyTest
    : TestBase
{
    // Fields
    private readonly DefaultFixture _fixture;

    // Constructors
    public DummyTest(
        ITestOutputHelper testOutputHelper,
        DefaultFixture fixture
    ) : base(testOutputHelper)
    {
        _fixture = fixture;
    }

    // Protected Methods
    protected override IDateTimeProvider CreateDateTimeProvider(DateTimeOffset currentDate)
    {
        var dateTimeProvider = new DateTimeProvider();

        dateTimeProvider.ChangeGetDateCustomFunction(() => currentDate);

        return dateTimeProvider;
    }

    [Fact]
    public void FixtureBase_Should_Correctly_Initialized()
    {
        // Assert
        _fixture.TenantId.Should().NotBe(Guid.Empty);
        _fixture.ExecutionUser.Should().NotBeNull();
        _fixture.SourcePlatform.Should().NotBeNull();
    }

    [Fact]
    public void FixtureBase_Should_Have_DependecyInjection_Configured()
    {
        _fixture.Should().NotBeNull();
        _fixture.CreateNewDependencyInjectionContainer().Resolve<IDummyService>().Should().NotBeNull();
    }

    [Fact]
    public void TestBase_Should_Have_TestOutputHelper_Instance()
    {
        TestOutputHelper.Should().NotBeNull();
    }

    [Fact]
    public void TestBase_Should_SetNewDateForDateTimeProvider()
    {
        // Arrange and Act
        var date1 = DateTimeProvider.GetDate();
        GenerateNewDateForDateTimeProvider();
        var date2 = DateTimeProvider.GetDate();
        var date3 = DateTimeProvider.GetDate();
        GenerateNewDateForDateTimeProvider();
        var date4 = DateTimeProvider.GetDate();
        var date5 = DateTimeProvider.GetDate();

        // Assert
        date1.Should().NotBe(date2);
        date1.Should().NotBe(date4);
        date2.Should().Be(date3);
        date4.Should().Be(date5);
        date2.Should().NotBe(date4);
    }

    [Fact]
    public async Task TestBase_Should_Set_DateTimeProvider_GetDateCustomFunction()
    {
        // Arrange and Act
        var firstDate = DateTimeProvider.GetDate();
        await Task.Delay(500);
        var secondDate = DateTimeProvider.GetDate();

        // Assert
        firstDate.Should().Be(secondDate);
    }

    [Fact]
    public void TestBase_Should_ValidateAfterRegisterNewOperation_Success()
    {
        // Arrange
        var executionUser = FixtureBase.GenerateNewExecutionUser();
        var sourcePlatform = FixtureBase.GenerateNewSourcePlatform();

        // Act
        var dummyDomainEntity = new DummyDomainEntity(DateTimeProvider).RegisterNew(
            tenantId: FixtureBase.GenerateNewTenantId(),
            executionUser,
            sourcePlatform
        );

        // Assert
        ValidateAfterRegisterNew(dummyDomainEntity, executionUser, sourcePlatform);
    }

    [Fact]
    public void TestBase_Should_ValidateAfterModificationOperation_Success()
    {
        // Arrange
        var executionUser = FixtureBase.GenerateNewExecutionUser();
        var sourcePlatform = FixtureBase.GenerateNewSourcePlatform();

        GenerateNewDateForDateTimeProvider();

        var dummyDomainEntity = new DummyDomainEntity(DateTimeProvider).RegisterNew(
            tenantId: FixtureBase.GenerateNewTenantId(),
            executionUser,
            sourcePlatform
        );
        var clonedDummyDomainEntity = dummyDomainEntity.DeepClone();

        GenerateNewDateForDateTimeProvider();

        // Act
        dummyDomainEntity.RegisterModification(executionUser, sourcePlatform);

        // Assert
        ValidateAfterRegisterModification(clonedDummyDomainEntity, dummyDomainEntity, executionUser, sourcePlatform);
    }

}