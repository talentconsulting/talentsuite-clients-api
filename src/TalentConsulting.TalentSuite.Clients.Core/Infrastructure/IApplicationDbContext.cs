using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Core.Infrastructure;

public interface IApplicationDbContext
{
    public DbSet<Client> Clients { get; }
    public DbSet<ClientProject> ClientProjects { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
