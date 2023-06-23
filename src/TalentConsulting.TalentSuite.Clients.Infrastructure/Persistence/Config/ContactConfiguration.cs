using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Config;

public class ContactConfiguration
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Firstname)
            .IsRequired();
        builder.Property(t => t.Email)
            .IsRequired();
        builder.Property(t => t.ReceivesReport)
            .IsRequired();
        builder.Property(t => t.ProjectId)
            .IsRequired();

        builder.Property(t => t.Created)
            .IsRequired();

    }
}
