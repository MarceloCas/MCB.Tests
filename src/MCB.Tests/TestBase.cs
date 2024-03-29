﻿using FluentAssertions;
using MCB.Core.Domain.Entities.DomainEntitiesBase;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using Xunit.Abstractions;

namespace MCB.Tests;

public abstract class TestBase
{
    // Properties
    protected ITestOutputHelper TestOutputHelper { get; }
    protected IDateTimeProvider DateTimeProvider { get; private set; }

    // Constructors
    protected TestBase(ITestOutputHelper testOutputHelper)
    {
        TestOutputHelper = testOutputHelper;
        DateTimeProvider = CreateDateTimeProvider(new DateTimeOffset(year: 2000, month: 1, day: 1, hour: 12, minute: 0, second: 0, offset: TimeSpan.Zero));
    }

    // Protected Abstract Methods
    protected abstract IDateTimeProvider CreateDateTimeProvider(DateTimeOffset currentDate);

    // Protected Methods
    protected void ValidateAfterRegisterNew(
        DomainEntityBase domainEntity,
        Guid tenantId,
        string executionUser,
        string sourcePlatform,
        Guid correlationId
    )
    {
        domainEntity.Id.Should().NotBe(Guid.Empty);
        domainEntity.TenantId.Should().Be(tenantId);

        domainEntity.AuditableInfo.CreatedAt.Should().Be(DateTimeProvider.GetDate().UtcDateTime);
        domainEntity.AuditableInfo.CreatedBy.Should().Be(executionUser);

        domainEntity.AuditableInfo.LastUpdatedAt.Should().BeNull();
        domainEntity.AuditableInfo.LastUpdatedBy.Should().BeNull();

        domainEntity.AuditableInfo.LastSourcePlatform.Should().Be(sourcePlatform);

        domainEntity.RegistryVersion.Should().Be(DateTimeProvider.GetDate().UtcDateTime);

        domainEntity.AuditableInfo.LastCorrelationId.Should().Be(correlationId);
    }
    protected void ValidateAfterRegisterModification(
        DomainEntityBase domainEntityBeforeModification,
        DomainEntityBase domainEntityAfterModification,
        string executionUser,
        string sourcePlatform
    )
    {
        domainEntityAfterModification.Should().NotBeSameAs(domainEntityBeforeModification);
        domainEntityAfterModification.Id.Should().Be(domainEntityBeforeModification.Id);

        domainEntityAfterModification.AuditableInfo.Should().NotBeSameAs(domainEntityBeforeModification.AuditableInfo);

        domainEntityAfterModification.AuditableInfo.CreatedAt.Should().Be(domainEntityBeforeModification.AuditableInfo.CreatedAt);
        domainEntityAfterModification.AuditableInfo.CreatedBy.Should().Be(domainEntityBeforeModification.AuditableInfo.CreatedBy);

        domainEntityAfterModification.AuditableInfo.LastUpdatedAt.Should().BeAfter(domainEntityAfterModification.AuditableInfo.CreatedAt);
        domainEntityAfterModification.AuditableInfo.LastUpdatedAt.Should().Be(DateTimeProvider.GetDate().UtcDateTime);
        domainEntityAfterModification.AuditableInfo.LastUpdatedBy.Should().Be(executionUser);

        domainEntityAfterModification.AuditableInfo.LastSourcePlatform.Should().Be(sourcePlatform);

        domainEntityAfterModification.RegistryVersion.Should().BeAfter(domainEntityBeforeModification.RegistryVersion);
    }
}
