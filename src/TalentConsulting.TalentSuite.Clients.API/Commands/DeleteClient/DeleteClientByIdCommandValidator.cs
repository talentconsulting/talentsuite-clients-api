using FluentValidation;

namespace TalentConsulting.TalentSuite.Clients.API.Commands.DeleteClient;

public class DeleteClientByIdCommandValidator : AbstractValidator<DeleteClientByIdCommand>
{
    public DeleteClientByIdCommandValidator()
    {
        RuleFor(v => v.Id)
            .MinimumLength(1)
            .NotNull()
            .NotEmpty();  
    }
}

