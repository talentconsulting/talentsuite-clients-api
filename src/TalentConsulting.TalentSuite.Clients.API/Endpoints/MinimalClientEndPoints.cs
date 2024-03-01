using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics.CodeAnalysis;
using TalentConsulting.TalentSuite.Clients.API.Commands.CreateClient;
using TalentConsulting.TalentSuite.Clients.API.Commands.UpdateClient;
using TalentConsulting.TalentSuite.Clients.API.Queries.GetClients;
using TalentConsulting.TalentSuite.Clients.Common.Entities;

namespace TalentConsulting.TalentSuite.Clients.API.Endpoints;

[ExcludeFromCodeCoverage]
public class MinimalClientEndPoints
{
    private readonly string[] tag = new string[] { "Clients" };
    public void RegisterClientEndPoints(WebApplication app)
    {
        app.MapPost("api/client", [Authorize(Policy = "TalentConsultingUser")] async ([FromBody] ClientDto request, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                CreateClientCommand command = new(request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Clients", "Create client") { Tags = tag });

        app.MapPut("api/client/{id}", [Authorize(Policy = "TalentConsultingUser")] async (string id, [FromBody] ClientDto request, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalClientEndPoints> logger) =>
        {
            try
            {
                UpdateClientCommand command = new(id, request);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred updating client (api). {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Update Client", "Update Client By Id") { Tags = tag });

        app.MapGet("api/clients", [Authorize(Policy = "TalentConsultingUser")] async (int? pageNumber, int? pageSize, CancellationToken cancellationToken, ISender _mediator) =>
        {
            try
            {
                GetClientsCommand request = new(pageNumber, pageSize);
                var result = await _mediator.Send(request, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Clients", "Get Clients Paginated") { Tags = tag });

        app.MapGet("api/client/{id}", [Authorize(Policy = "TalentConsultingUser")] async (string id, CancellationToken cancellationToken, ISender _mediator, ILogger<MinimalClientEndPoints> logger) =>
        {
            try
            {
                GetClientByIdCommand command = new(id);
                var result = await _mediator.Send(command, cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred getting client (api). {exceptionMessage}", ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }).WithMetadata(new SwaggerOperationAttribute("Get Client", "Get Client By Id") { Tags = tag });
    }
}
