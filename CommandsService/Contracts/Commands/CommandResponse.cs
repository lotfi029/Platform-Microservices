namespace CommandsService.Contracts.Commands;

public record CommandResponse(
    Guid Id,
    string HowTo,
    string CommandLine,
    Guid PlatformId
);

