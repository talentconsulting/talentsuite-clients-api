namespace TalentConsulting.TalentSuite.Clients.Common.Interfaces;

public interface IHandle<in T> where T : DomainEventBase
{
    Task HandleAsync(T args);
}