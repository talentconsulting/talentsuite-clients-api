using Ardalis.GuardClauses;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Xml.Serialization;
using TalentConsulting.TalentSuite.Clients.API.Commands.CreateClient;
using TalentConsulting.TalentSuite.Clients.API.Commands.DeleteClient;
using TalentConsulting.TalentSuite.Clients.API.Commands.UpdateClient;
using TalentConsulting.TalentSuite.Clients.API.Queries.GetClients;
using TalentConsulting.TalentSuite.Clients.Common.Entities;
using TalentConsulting.TalentSuite.Clients.Core.Entities;

namespace TalentConsulting.TalentSuite.Clients.UnitTests.Clients;

public class WhenUsingClientCommands : BaseCreateDbUnitTest
{
    [Fact]
    public async Task ThenCreateClient()
    {
        //Arrange
        var logger = new Mock<ILogger<CreateClientCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        ClientDto testProject = GetTestClientDto();
        var command = new CreateClientCommand(testProject);
        var handler = new CreateClientCommandHandler(mockApplicationDbContext, _mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testProject.Id);
    }

    [Fact]
    public async Task ThenCreateClientWithEmptyClientProjects()
    {
        //Arrange
        var logger = new Mock<ILogger<CreateClientCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        ClientDto testProject = GetTestClientDto(true);
        var command = new CreateClientCommand(testProject);
        var handler = new CreateClientCommandHandler(mockApplicationDbContext, _mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testProject.Id);
    }

    [Fact]
    public async Task ThenHandle_ShouldThrowInvalidOperationException_WhenAlreadyExists()
    {
        var logger = new Mock<ILogger<CreateClientCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbClient = new Client(_clientId, "Name", "Contact Name", "Contact Email", new List<ClientProject>() { new ClientProject(_clientProjectId, _clientId, _projectId) });
        mockApplicationDbContext.Clients.Add(dbClient);
        await mockApplicationDbContext.SaveChangesAsync();
        ClientDto testProject = GetTestClientDto();
        var command = new CreateClientCommand(testProject);
        var handler = new CreateClientCommandHandler(mockApplicationDbContext, _mapper, logger.Object);

        // Act
        //Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task ThenHandle_ShouldThrowArgumentNullException_WhenEntityIsNull()
    {
        // Arrange
        var logger = new Logger<CreateClientCommandHandler>(new LoggerFactory());
        var handler = new CreateClientCommandHandler(GetApplicationDbContext(), _mapper, logger);
        var command = new CreateClientCommand(default!);

        // Act
        //Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task ThenUpdateClient()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbClient = new Client(_clientId, "Name", "Contact Name", "Contact Email", new List<ClientProject>() { new ClientProject(_clientProjectId, _clientId, _projectId) });
        mockApplicationDbContext.Clients.Add(dbClient);
        await mockApplicationDbContext.SaveChangesAsync();
        var testClient = GetTestClientDto();
        var logger = new Mock<ILogger<UpdateClientCommandHandler>>();

        var command = new UpdateClientCommand(_clientId.ToString(), testClient);
        var handler = new UpdateClientCommandHandler(mockApplicationDbContext, _mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testClient.Id);
    }

    [Fact]
    public async Task ThenUpdateClientWithNullCommand()
    {
        // Arrange
        var logger = new Mock<ILogger<UpdateClientCommandHandler>>();
        var handler = new UpdateClientCommandHandler(GetApplicationDbContext(), _mapper, logger.Object);


        // Act
        //Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(null, CancellationToken.None));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    }
    

    [Fact]
    public async Task ThenHandle_ThrowsException_WhenClientNotFound()
    {
        // Arrange
        var dbContext = GetApplicationDbContext();
        var logger = new Mock<ILogger<UpdateClientCommandHandler>>();
        var handler = new UpdateClientCommandHandler(dbContext, _mapper, logger.Object);
        var command = new UpdateClientCommand("someotherid", default!);

        // Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

    }

    [Fact]
    public async Task ThenGetClients()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbClient = GetTestClient();
        mockApplicationDbContext.Clients.Add(dbClient);
        await mockApplicationDbContext.SaveChangesAsync();


        var command = new GetClientsCommand(1, 99);
        var handler = new GetClientsCommandHandler(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items[0].Id.Should().Be(dbClient.Id.ToString());
        result.Items[0].Name.Should().Be(dbClient.Name);

    }

    [Fact]
    public async Task ThenGetClientWithNullRequest()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbClient = GetTestClient();
        mockApplicationDbContext.Clients.Add(dbClient);
        await mockApplicationDbContext.SaveChangesAsync();
        var handler = new GetClientsCommandHandler(mockApplicationDbContext);

        //Act
        var result = await handler.Handle(new GetClientsCommand(1, 99), new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Items[0].Id.Should().Be(dbClient.Id.ToString());
        result.Items[0].Name.Should().Be(dbClient.Name);
    }

    [Fact]
    public async Task ThenGetClientById()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbClient = GetTestClient();
        mockApplicationDbContext.Clients.Add(dbClient);
        await mockApplicationDbContext.SaveChangesAsync();
        var expectedClient = _mapper.Map<ClientDto>(dbClient);


        var command = new GetClientByIdCommand(dbClient.Id.ToString());
        var handler = new GetClientByIdCommandHandler(mockApplicationDbContext, _mapper);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(dbClient.Id.ToString());
        expectedClient.Should().BeEquivalentTo(result);
        
    }

    [Fact]
    public async Task ThenGetClientByIdHandle_ThrowsException_WhenClientNotFound()
    {
        // Arrange
        var dbContext = GetApplicationDbContext();
        var handler = new GetClientByIdCommandHandler(dbContext, _mapper);
        var command = new GetClientByIdCommand("someotherid");

        // Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command, CancellationToken.None));

    }

    [Fact]
    public async Task AttachExistingClientProjects_Returns_ExistingClientProjects()
    {
        //Arrange
        var existingProjects = new List<ClientProject>
        {
            new ClientProject(_clientProjectId, _clientId, _projectId)
        };
        var logger = new Mock<ILogger<CreateClientCommandHandler>>();
        var mockApplicationDbContext = GetApplicationDbContext();
        mockApplicationDbContext.ClientProjects.AddRange(existingProjects);
        await mockApplicationDbContext.SaveChangesAsync();
        ClientDto testProject = GetTestClientDto();
        var command = new CreateClientCommand(testProject);
        var handler = new CreateClientCommandHandler(mockApplicationDbContext, _mapper, logger.Object);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().NotBeNull();
        result.Should().Be(testProject.Id);

    }


    [Fact]
    public async Task ThenDeleteClientById()
    {
        var mockApplicationDbContext = GetApplicationDbContext();
        var dbClient = GetTestClient();
        mockApplicationDbContext.Clients.Add(dbClient);
        var clientProject = mockApplicationDbContext.ClientProjects.Find(_clientProjectId);
        var project = GetTestProject();
        project.ClientProjects = new List<ClientProject>() { clientProject };
        mockApplicationDbContext.Projects.Add(project);
        await mockApplicationDbContext.SaveChangesAsync();

        var command = new DeleteClientByIdCommand(dbClient.Id.ToString());
        var handler = new DeleteClientByIdCommandHandler(mockApplicationDbContext, _mapper);

        //Act
        var result = await handler.Handle(command, new CancellationToken());

        //Assert
        result.Should().Be(true);

    }


    public static Client GetTestClient()
    {
        return new Client(_clientId, "Name", "Contact Name", "Contact Email", new List<ClientProject>() { new ClientProject(_clientProjectId, _clientId, _projectId) });
    }

    public static ClientDto GetTestClientDto(bool emptyClientProjects = false)
    {
        if (emptyClientProjects)
        {
            return new ClientDto(_clientId.ToString(), "Name", "Contact Name", "Contact Email", new List<ClientProjectDto>());
        }
        return new ClientDto(_clientId.ToString(), "Name", "Contact Name", "Contact Email", new List<ClientProjectDto>() { new ClientProjectDto(_clientProjectId.ToString(), _clientId.ToString(), _projectId.ToString()) });
    }

    public static Project GetTestProject()
    {
        var sowFile = new SowFile
        {
            Id = _sowFileId,
            File = new byte[] { 1, 2, 3, 4, 5 },
            Mimetype = "application/pdf",
            Filename = "test.pdf",
            Size = 1234,
            SowId = _sowId.ToString()
        };
        var sows = new List<Sow>() { new Sow(_sowId, DateTime.Now, new List<SowFile>(){ sowFile },true,DateTime.Now,DateTime.Now.AddDays(7),_projectId) };

        return new Project(_projectId, "Contract Number", "Name", "Reference", DateTime.Now, DateTime.Now.AddDays(7),
            new List<ClientProject>() { new ClientProject(_clientProjectId, _clientId, _projectId) },
            new List<Contact>() { new Contact(_contactId, "Firstname", "Email", true, _projectId) },
            new List<Report>() { new Report(_reportId, "Planned Tasks", "Completed Tasks", 1, DateTime.Now, _projectId, _userId,
            new List<Risk>() { new Risk(_riskId, _reportId, "Risk Details", "Risk Mitigation", "RagStatus" ) }) }, sows);
    }
}
