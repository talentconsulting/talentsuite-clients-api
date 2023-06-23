using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Config;

public class SowConfiguration
{
    public void Configure(EntityTypeBuilder<Sow> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.File)
            .IsRequired();
        builder.Property(t => t.IsChangeRequest)
            .IsRequired();
        builder.Property(t => t.SowStartDate)
            .IsRequired();
        builder.Property(t => t.SowEndDate)
            .IsRequired();
        builder.Property(t => t.ProjectId)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();
    }
}
