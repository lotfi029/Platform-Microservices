namespace CommandsService.Contracts.Commands;

public record CreateCommandRequest(
    string HowTo,
    string CommandLine
);
