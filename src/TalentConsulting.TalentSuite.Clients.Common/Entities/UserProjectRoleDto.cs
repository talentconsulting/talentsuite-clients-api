using System.Reflection.Metadata;

namespace TalentConsulting.TalentSuite.Clients.Common.Entities;

public record UserProjectRoleDto
{
    private UserProjectRoleDto() { }

    public UserProjectRoleDto(string id, string userid, string projectid)
    {
        Id = id;
        UserId = userid;
        ProjectId = projectid;
    }

    public string Id { get; init; } = default!;
    public string UserId { get; init; } = default!;
    public string ProjectId { get; init; } = default!;

}