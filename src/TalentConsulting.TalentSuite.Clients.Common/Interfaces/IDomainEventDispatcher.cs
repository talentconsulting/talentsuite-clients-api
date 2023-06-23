namespace TalentConsulting.TalentSuite.Clients.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<EntityBase<string>> entitiesWithEvents);
}
