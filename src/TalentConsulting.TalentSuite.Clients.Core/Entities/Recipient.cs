using System.ComponentModel.DataAnnotations.Schema;
using TalentConsulting.TalentSuite.Clients.Common;
using TalentConsulting.TalentSuite.Clients.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Clients.Core.Entities;

[Table("recipients")]
public class Recipient : EntityBase<string>, IAggregateRoot
{
    private Recipient() { }

    public Recipient(string id, string name, string email, string notificationid)
    {
        Id = id;
        Name = name;
        Email = email;
        Notificationid = notificationid;
    }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Notificationid { get; set; } = null!;
#if ADD_ENTITY_NAV
    public virtual Notification Notification { get; set; } = null!;
#endif


}