using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.Clients.Core.Entities;
using TalentConsulting.TalentSuite.Clients.Core.Infrastructure;
using TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Repository;

namespace TalentConsulting.TalentSuite.Clients.API.Commands.DeleteClient;

public class DeleteClientByIdCommand : IRequest<bool>
{
    public DeleteClientByIdCommand(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}

#pragma warning disable CA1859
public class DeleteClientByIdCommandHandler : IRequestHandler<DeleteClientByIdCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteClientByIdCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteClientByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients
            .Include(x => x.ClientProjects)
            .ThenInclude(cp => cp.Project)
            .FirstOrDefaultAsync(x => x.Id.ToString() == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Client), request.Id.ToString());
        }

        try
        {
            var projects = entity.ClientProjects.Select(x => x.Project).ToList();
            _context.Projects.RemoveRange(projects);

            // Delete the client
            _context.Clients.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

}

#pragma warning restore CA1859