namespace CommandsService.Contracts.Commands;

public class CreateCommandRequestValidator : AbstractValidator<CreateCommandRequest> 
{
    public CreateCommandRequestValidator()
    {
        RuleFor(x => x.HowTo)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.CommandLine)
            .NotEmpty()
            .NotNull();
    }
}

