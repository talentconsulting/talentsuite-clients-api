using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

public class DeleteClientByIdCommandHandler : IRequestHandler<DeleteClientByIdCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IDbContextTransaction _transaction;

    public DeleteClientByIdCommandHandler(ApplicationDbContext context, IMapper mapper, IDbContextTransaction transaction)
    {
        _context = context;
        _transaction = transaction;
    }

    public async Task<bool> Handle(DeleteClientByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients
            .Include(x => x.ClientProjects)
            .FirstOrDefaultAsync(x => x.Id.ToString() == request.Id, cancellationToken: cancellationToken);


        if (entity == null)
        {
            throw new NotFoundException(nameof(Client), request.Id.ToString());
        }

        var projects = await _context.Projects
            .Include(x => x.ClientProjects)
            .Include(x => x.Contacts)
            .Include(x => x.Reports)
            .ThenInclude(x => x.Risks)
            .Include(x => x.Sows)
            .ThenInclude(x => x.Files)
            .Where(x => x.ClientProjects.Any(x => x.ClientId.ToString() == request.Id)).ToListAsync(cancellationToken);


        var reports = new List<Report>();
        
        foreach (var project in projects)
        {
            reports.AddRange(await _context.Reports.Where(x => x.ProjectId.ToString() == project.Id.ToString()).ToListAsync(cancellationToken));
        }

           
        try
        {
            RemoveEntities(reports, projects, entity);

            await _context.SaveChangesAsync(cancellationToken);

            await _transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _transaction.RollbackAsync(cancellationToken);
            return false;
        }
        

        return true;
    }

    
    private void RemoveEntities(List<Report> reports, List<Project> projects, Client entity)
    {
        _context.Reports.RemoveRange(reports);
        _context.Projects.RemoveRange(projects);
        _context.Clients.Remove(entity);
    }
    
}