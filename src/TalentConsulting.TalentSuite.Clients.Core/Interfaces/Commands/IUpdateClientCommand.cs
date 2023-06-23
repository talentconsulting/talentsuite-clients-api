using TalentConsulting.TalentSuite.Clients.Common.Entities;

namespace TalentConsulting.TalentSuite.Clients.Core.Interfaces.Commands;

public interface IUpdateClientCommand
{
    string Id { get; }
    ClientDto ClientDto { get; }
}
