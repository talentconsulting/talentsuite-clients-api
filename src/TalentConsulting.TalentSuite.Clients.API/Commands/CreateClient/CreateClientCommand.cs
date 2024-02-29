using AutoMapper;
using MediatR;
using TalentConsulting.TalentSuite.Clients.Common.Entities;
using TalentConsulting.TalentSuite.Clients.Core.Entities;
using TalentConsulting.TalentSuite.Clients.Core.Interfaces.Commands;
using TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Repository;

namespace TalentConsulting.TalentSuite.Clients.API.Commands.CreateClient;

public class CreateClientCommand : IRequest<string>, ICreateClientCommand
{
    public CreateClientCommand(ClientDto reportDto)
    {
        ClientDto = reportDto;
    }

    public ClientDto ClientDto { get; }
}

public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, string>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateClientCommandHandler> _logger;
    public CreateClientCommandHandler(ApplicationDbContext context, IMapper mapper, ILogger<CreateClientCommandHandler> logger)
    {
        _logger = logger;
        _context = context;
        _mapper = mapper;
    }
    public async Task<string> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var unsavedEntity = _mapper.Map<Client>(request.ClientDto);
            ArgumentNullException.ThrowIfNull(unsavedEntity);

            var existing = _context.Clients.FirstOrDefault(e => unsavedEntity.Id == e.Id);

            if (existing is not null)
                throw new InvalidOperationException($"Client with Id: {unsavedEntity.Id} already exists, Please use Update command");

            unsavedEntity.ClientProjects = AttachExistingClientProjects(unsavedEntity.ClientProjects);
#if USE_DISPATCHER
            unsavedEntity.RegisterDomainEvent(new ClientCreatedEvent(unsavedEntity));
#endif
            _context.Clients.Add(unsavedEntity);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating a client. {exceptionMessage}", ex.Message);
            throw;
        }

        if (request.ClientDto is not null)
            return request.ClientDto.Id;
        else
            return string.Empty;
    }

    private List<ClientProject> AttachExistingClientProjects(ICollection<ClientProject>? unSavedEntities)
    {
        var returnList = new List<ClientProject>();

        if (unSavedEntities is null || unSavedEntities.Count == 0)
            return returnList;

        var existing = _context.ClientProjects.Where(e => unSavedEntities.Select(c => c.Id).Contains(e.Id)).ToList();

        for (var i = 0; i < unSavedEntities.Count; i++)
        {
            var unSavedItem = unSavedEntities.ElementAt(i);
            var savedItem = existing.Find(x => x.Id == unSavedItem.Id);
            returnList.Add(savedItem ?? unSavedItem);
        }

        return returnList;
    }
}
