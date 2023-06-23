using TalentConsulting.TalentSuite.Clients.Common.Entities;

namespace TalentConsulting.TalentSuite.Clients.Core.Interfaces.Commands;

public interface ICreateClientCommand
{
    ClientDto ClientDto { get; }
}