using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Config;

public class ReportConfiguration
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.Property(t => t.Id)
            .IsRequired();
        builder.Property(t => t.PlannedTasks)
            .IsRequired();
        builder.Property(t => t.CompletedTasks)
            .IsRequired();
        builder.Property(t => t.Weeknumber)
            .IsRequired();
        builder.Property(t => t.SubmissionDate)
            .IsRequired();
        builder.Property(t => t.ProjectId)
            .IsRequired();
        builder.Property(t => t.Created)
            .IsRequired();


        builder.HasMany(s => s.Risks)
            .WithOne()
            .HasForeignKey(lc => lc.ReportId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
