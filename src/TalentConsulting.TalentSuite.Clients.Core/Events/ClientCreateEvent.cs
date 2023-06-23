using TalentConsulting.TalentSuite.Clients.Common;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Core.Events;

public interface IClientCreatedEvent
{
    Client Item { get; }
}

public class ClientCreatedEvent : DomainEventBase, IClientCreatedEvent
{ 
    public ClientCreatedEvent(Client item)
    {
        Item = item;
    }

    public Client Item { get; }
}
