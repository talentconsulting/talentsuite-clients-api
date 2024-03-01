using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using TalentConsulting.TalentSuite.Clients.Common.Entities;
using TalentConsulting.TalentSuite.Clients.Core.Entities;
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
    private readonly ApplicationDbContext _context;

    public DeleteClientByIdCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteClientByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Clients
            .Include(x => x.ClientProjects)
            .FirstOrDefaultAsync(x => x.Id.ToString() == request.Id, cancellationToken: cancellationToken);


        if (entity == null)
        {
            throw new NotFoundException(nameof(ReportDto), request.Id.ToString());
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

        //This is done so we can use functional tests with in-memory database which does not support transactions
        if (_context.Database.IsInMemory()) // Check if using in-memory database
        {
            try
            {
                RemoveEntities(reports, projects, entity);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                return false;
            }
        }
        else // Use transactions if not using in-memory database
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                RemoveEntities(reports, projects, entity);

                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return false;
            }
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