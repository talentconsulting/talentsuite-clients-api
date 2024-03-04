using System.ComponentModel.DataAnnotations.Schema;
using TalentConsulting.TalentSuite.Clients.Common;
using TalentConsulting.TalentSuite.Clients.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Clients.Core.Entities;

[Table("clientprojects")]
public class ClientProject : EntityBase<Guid>, IAggregateRoot
{
    private ClientProject() { }

    public ClientProject(Guid id, Guid clientid, Guid projectid)
    {
        Id = id;
        ClientId = clientid;
        ProjectId = projectid;
    }

    public Guid ClientId { get; set; }
    public Guid ProjectId { get; set; }

    public virtual Project Project { get; set; } = null!;

#if ADD_ENTITY_NAV
    public virtual Client Client { get; set; } = null!;
#endif

}