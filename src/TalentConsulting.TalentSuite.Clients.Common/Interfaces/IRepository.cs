using Ardalis.Specification;

namespace TalentConsulting.TalentSuite.Clients.Common.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
}
