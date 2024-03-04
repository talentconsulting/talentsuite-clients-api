using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace TalentConsulting.TalentSuite.Clients.Common;

[ExcludeFromCodeCoverage]
public abstract class DomainEventBase : INotification
{
    public DateTimeOffset DateOccurred { get; protected set; } = DateTimeOffset.UtcNow;
}

