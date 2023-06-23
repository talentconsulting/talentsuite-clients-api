using TalentConsulting.TalentSuite.Clients.Common;
using TalentConsulting.TalentSuite.Clients.Common.Interfaces;

namespace TalentConsulting.TalentSuite.Clients.Core.Entities;

public class Audit : EntityBase<string>, IAggregateRoot
{
    private Audit() { }

    public Audit(string id, string detail, string userid)
    {
        Id = id;
        Detail = detail;
        UserId = userid;
    }

    public string Detail { get; init; } = null!;
    public string UserId { get; init; } = null!;
}