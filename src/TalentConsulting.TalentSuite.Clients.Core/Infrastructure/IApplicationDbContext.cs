using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Core.Infrastructure;

public interface IApplicationDbContext
{
    public DbSet<Report> Reports { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
