using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Config;

public class RecipientConfiguration
{
    public void Configure(EntityTypeBuilder<Recipient> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.Name)
            .IsRequired();
        builder.Property(t => t.Email)
            .IsRequired();
        builder.Property(t => t.Notificationid)
            .IsRequired();

        builder.Property(t => t.Created)
            .IsRequired();
    }
}
