namespace TalentConsulting.TalentSuite.Clients.Common.Interfaces;

public interface IHandle<T> where T : DomainEventBase
{
    Task HandleAsync(T args);
}
