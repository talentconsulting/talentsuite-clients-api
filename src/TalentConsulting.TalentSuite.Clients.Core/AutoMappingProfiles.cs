using AutoMapper;
using TalentConsulting.TalentSuite.Clients.Common.Entities;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.Core;

public class AutoMappingProfiles : Profile
{
    public AutoMappingProfiles()
    {
        CreateMap<ClientDto, Client>().ReverseMap();
        CreateMap<ClientProjectDto, ClientProject>().ReverseMap();
    }
}
