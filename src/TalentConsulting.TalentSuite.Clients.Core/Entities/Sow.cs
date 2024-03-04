﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using TalentConsulting.TalentSuite.Clients.Common;
using TalentConsulting.TalentSuite.Clients.Common.Interfaces;


namespace TalentConsulting.TalentSuite.Clients.Core.Entities;

[Table("sows")]
public class Sow : EntityBaseEx<Guid>, IAggregateRoot
{
    private Sow() { }

    public Sow(Guid id, DateTime created, ICollection<SowFile> files, bool ischangerequest, DateTime sowstartdate, DateTime sowenddate, Guid projectid)
    {
        Id = id;
        Created = created;
        IsChangeRequest = ischangerequest;
        SowStartDate = sowstartdate;
        SowEndDate = sowenddate;
        ProjectId = projectid;
        Files = files;
    }

    public bool IsChangeRequest { get; set; }
    [Column("sow_startdate")]
    public DateTime SowStartDate { get; set; }
    [Column("sow_enddate")]
    public DateTime SowEndDate { get; set; }
    public Guid ProjectId { get; set; }
#if ADD_ENTITY_NAV
    public virtual Project Project { get; set; } = null!;
#endif

    public virtual ICollection<SowFile> Files { get; set; } = new Collection<SowFile>();

}
