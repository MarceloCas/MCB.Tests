using MCB.Core.Domain.Entities.DomainEntitiesBase;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;
using System;

namespace MCB.Tests.Tests.DomainEntities;

public class DummyDomainEntity
    : DomainEntityBase
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public DummyDomainEntity(IDateTimeProvider dateTimeProvider) 
        : base(dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    protected override DomainEntityBase CreateInstanceForCloneInternal() => new DummyDomainEntity(_dateTimeProvider);

    public DummyDomainEntity RegisterNew(Guid tenantId, string executionUser, string sourcePlatform, Guid correlationId)
        => RegisterNewInternal<DummyDomainEntity>(tenantId, executionUser, sourcePlatform, correlationId);
    public DummyDomainEntity RegisterModification(string executionUser, string sourcePlatform, Guid correlationId)
        => RegisterModificationInternal<DummyDomainEntity>(executionUser, sourcePlatform, correlationId);

    public DummyDomainEntity DeepClone()
        => DeepCloneInternal<DummyDomainEntity>();
}
