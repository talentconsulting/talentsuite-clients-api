using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Core.Infrastructure;

public interface IApplicationDbContext
{
    DbSet<Client> Clients { get; }
    DbSet<ClientProject> ClientProjects { get; }
    DbSet<Contact> Contacts { get; }
    DbSet<Project> Projects { get; }
    DbSet<Report> Reports { get; }
    DbSet<Risk> Risks { get; }
    DbSet<Sow> Sows { get; }
    DbSet<SowFile> SowFiles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
