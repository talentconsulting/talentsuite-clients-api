﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TalentConsulting.TalentSuite.Clients.Common;

/// <summary>
/// Base types for all Entities which track state using a given Id.
/// </summary>
public abstract class EntityBase<Tid>
{
    public Tid Id { get; set; } = default!;

    public DateTime? Created { get; set; } = default;

    private readonly List<DomainEventBase> _domainEvents = new();

    [NotMapped]
    public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    public void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);

    internal void ClearDomainEvents() => _domainEvents.Clear();
}

