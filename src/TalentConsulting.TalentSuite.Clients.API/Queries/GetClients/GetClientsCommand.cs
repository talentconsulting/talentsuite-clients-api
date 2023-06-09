﻿using Ardalis.GuardClauses;
using MediatR;
using TalentConsulting.TalentSuite.Clients.Common.Entities;
using TalentConsulting.TalentSuite.Clients.Common;
using TalentConsulting.TalentSuite.Clients.Core.Entities;
using TalentConsulting.TalentSuite.Clients.Core.Helpers;
using TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;

namespace TalentConsulting.TalentSuite.Clients.API.Queries.GetClients;

public class GetClientsCommand : IRequest<PaginatedList<ClientDto>>
{
    public GetClientsCommand(int? pageNumber, int? pageSize)
    {
        PageNumber = pageNumber != null ? pageNumber.Value : 1;
        PageSize = pageSize != null ? pageSize.Value : 1;
    }

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetClientsCommandHandler : IRequestHandler<GetClientsCommand, PaginatedList<ClientDto>>
{
    private readonly ApplicationDbContext _context;

    public GetClientsCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<PaginatedList<ClientDto>> Handle(GetClientsCommand request, CancellationToken cancellationToken)
    {
        var entities = _context.Clients
            .Include(x => x.ClientProjects);

        if (entities == null)
        {
            throw new NotFoundException(nameof(Client), "Clients");
        }

        var filteredClients = await entities.Select(x => new ClientDto(x.Id, x.Name, x.ContactName, x.ContactEmail, EntityToDtoHelper.GetClientProjects(x.ClientProjects))).ToListAsync();

        if (request != null)
        {
            var pageList = filteredClients.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            var result = new PaginatedList<ClientDto>(pageList, filteredClients.Count, request.PageNumber, request.PageSize);

            return result;
        }

        return new PaginatedList<ClientDto>(filteredClients, filteredClients.Count, 1, 10);
    }
}