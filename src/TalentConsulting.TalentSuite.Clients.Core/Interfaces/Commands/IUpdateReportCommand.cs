using TalentConsulting.TalentSuite.Clients.Common.Entities;

namespace TalentConsulting.TalentSuite.Clients.Core.Interfaces.Commands;


public interface IUpdateReportCommand
{
    string Id { get; }
    ReportDto ReportDto { get; }
}

