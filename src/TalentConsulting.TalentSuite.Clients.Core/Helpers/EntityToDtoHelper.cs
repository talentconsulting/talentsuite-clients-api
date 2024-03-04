using Ardalis.Specification;
using TalentConsulting.TalentSuite.Clients.Common.Entities;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Core.Helpers;

public static class EntityToDtoHelper
{
    public static List<ClientProjectDto> GetClientProjects(ICollection<ClientProject> clientProjects)
    {
        return clientProjects.Select(x => new ClientProjectDto(x.Id.ToString(), x.ClientId.ToString(), x.ProjectId.ToString())).ToList();
    }
}
