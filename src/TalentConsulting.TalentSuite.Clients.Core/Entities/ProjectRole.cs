using System.ComponentModel.DataAnnotations.Schema;
using TalentConsulting.TalentSuite.Clients.Common;
using TalentConsulting.TalentSuite.Clients.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Clients.Core.Entities;

[Table("projectroles")]
public class ProjectRole : EntityBase<string>, IAggregateRoot
{
    private ProjectRole() { }

    public ProjectRole(string id, string name, bool technical, string description)
    {
        Id = id;
        Name = name;
        Technical = technical;
        Description = description;
    }

    public string Name { get; set; } = null!;
    public bool Technical { get; set; }
    public string Description { get; set; } = null!;
}
