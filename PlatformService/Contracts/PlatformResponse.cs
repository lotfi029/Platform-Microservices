namespace PlatformService.Contracts;

public record PlatformResponse(
    Guid Id,
    string Name,
    string Publisher,
    string Cost
    );