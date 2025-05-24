namespace PlatformService.Contracts;

public record CreatePlatformRequest (
    string Name,
    string Publisher,
    string Cost
    );
