using Ardalis.GuardClauses;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.Clients.Common.Entities;
using TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Repository;

namespace TalentConsulting.TalentSuite.Clients.API.Queries.GetClients;

public class GetClientByIdCommand : IRequest<ClientDto>
{
    public GetClientByIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

public class GetClientByIdCommandHandler : IRequestHandler<GetClientByIdCommand, ClientDto>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetClientByIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ClientDto> Handle(GetClientByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients
            .Include(x => x.ClientProjects)
            .ProjectTo<ClientDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);


        if (entity == null)
        {
            throw new NotFoundException(nameof(ReportDto), request.Id.ToString());
        }

        return entity;

    }
}
