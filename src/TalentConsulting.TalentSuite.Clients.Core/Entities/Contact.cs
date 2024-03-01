using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using TalentConsulting.TalentSuite.Clients.Common;
using TalentConsulting.TalentSuite.Clients.Common.Interfaces;


namespace TalentConsulting.TalentSuite.Clients.Core.Entities;

[Table("contacts")]
public class Contact : EntityBase<Guid>, IAggregateRoot
{
    private Contact() { }

    public Contact(Guid id, string firstname, string email, bool receivesreport, Guid projectId)
    {
        Id = id;
        Firstname = firstname;
        Email = email;
        ReceivesReport = receivesreport;
        ProjectId = projectId;
    }

    public string Firstname { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool ReceivesReport { get; set; } = default!;
    public Guid ProjectId { get; set; }
#if ADD_ENTITY_NAV
    public virtual Project Project { get; set; } = null!;
#endif

}


