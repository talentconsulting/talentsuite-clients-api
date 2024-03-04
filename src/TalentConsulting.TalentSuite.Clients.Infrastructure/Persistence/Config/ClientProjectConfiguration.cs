using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Config;

[ExcludeFromCodeCoverage]
public static class ClientProjectConfiguration
{
    public static void Configure(EntityTypeBuilder<ClientProject> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.ClientId)
            .IsRequired();
        builder.Property(t => t.ProjectId)
            .IsRequired();
    }
}
