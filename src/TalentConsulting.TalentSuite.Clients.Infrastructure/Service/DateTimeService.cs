using System.Diagnostics.CodeAnalysis;
using TalentConsulting.TalentSuite.Clients.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Clients.Infrastructure.Service;

[ExcludeFromCodeCoverage]
public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}

