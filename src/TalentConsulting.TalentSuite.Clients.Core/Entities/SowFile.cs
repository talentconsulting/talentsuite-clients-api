using TalentConsulting.TalentSuite.Clients.Common;

namespace TalentConsulting.TalentSuite.Clients.Core.Entities;

public class SowFile : EntityBase<Guid>
{
    public required string Mimetype { get; set; }
    public required string Filename { get; set; }
    public required int Size { get; set; }
    public required string SowId { get; set; }
    public required byte[] File { get; set; }
}
