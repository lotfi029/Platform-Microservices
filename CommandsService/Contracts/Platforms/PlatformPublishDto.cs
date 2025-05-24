namespace CommandsService.Contracts.Platforms;

public record PlatformPublishDto( 
    Guid Id,
    string Name,
    string Event
);
