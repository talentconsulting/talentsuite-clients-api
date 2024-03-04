using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.Clients.Common.Entities;
using TalentConsulting.TalentSuite.Clients.Core.Entities;
using TalentConsulting.TalentSuite.Clients.Core.Interfaces.Commands;
using TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Repository;

namespace TalentConsulting.TalentSuite.Clients.API.Commands.UpdateClient;

public class UpdateClientCommand : IRequest<string>, IUpdateClientCommand
{
    public UpdateClientCommand(string id, ClientDto reportDto)
    {
        Id = id;
        ClientDto = reportDto;
    }

    public string Id { get; }
    public ClientDto ClientDto { get; }
}

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateClientCommandHandler> _logger;
    public UpdateClientCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<UpdateClientCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }
    public async Task<string> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.Clients.AsNoTracking()
            .Include(x => x.ClientProjects)
            .FirstOrDefault(x => x.Id.ToString() == request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Client), request.Id);
        }

        try
        {
            _mapper.Map(request.ClientDto, entity);
            ArgumentNullException.ThrowIfNull(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred updating Client. {exceptionMessage}", ex.Message);
            throw;
        }

        if (request is not null && request.ClientDto is not null)
            return request.ClientDto.Id;
        else
            return string.Empty;
    }
}
