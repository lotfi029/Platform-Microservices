using FluentValidation;

namespace PlatformService.Contracts;

public class CreatePlatformRequestValidator : AbstractValidator<CreatePlatformRequest>
{
    public CreatePlatformRequestValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty();
        
        RuleFor(e => e.Publisher)
            .NotEmpty();
        
        RuleFor(e => e.Cost)
            .NotEmpty();
    }
}