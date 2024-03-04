using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentConsulting.TalentSuite.Clients.API.Commands.DeleteClient;
using TalentConsulting.TalentSuite.Clients.API.Commands.UpdateClient;
using TalentConsulting.TalentSuite.Clients.Common.Entities;

namespace TalentConsulting.TalentSuite.Clients.UnitTests.Clients;

public class WhenValidatingDeleteClient : BaseTestValidation
{
    [Fact]
    public void ThenShouldNotErrorWhenIdIsProvidedWhenDeletingClients()
    {
        //Arrange
        var validator = new DeleteClientByIdCommandValidator();
        var testModel = new DeleteClientByIdCommand(_clientId);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Any().Should().BeFalse();
    }

    [Fact]
    public void ThenShouldErrorWhenModelHasNoIdWhenDeleting()
    {
        //Arrange
        var validator = new DeleteClientByIdCommandValidator();
        var testModel = new DeleteClientByIdCommand(default!);

        //Act
        var result = validator.Validate(testModel);

        //Assert
        result.Errors.Exists(x => x.PropertyName == "Id").Should().BeTrue();
    }
}
