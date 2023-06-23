using TalentConsulting.TalentSuite.Clients.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Clients.Infrastructure.Service;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}

