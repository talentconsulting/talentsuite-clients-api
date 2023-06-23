using Ardalis.Specification;

namespace TalentConsulting.TalentSuite.Clients.Common.Interfaces;


public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}
