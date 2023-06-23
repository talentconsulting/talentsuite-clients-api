using TalentConsulting.TalentSuite.Clients.Common.Entities;

namespace TalentConsulting.TalentSuite.Clients.Core.Interfaces.Commands;

public interface ICreateReportCommand
{
    ReportDto ReportDto { get; }
}
