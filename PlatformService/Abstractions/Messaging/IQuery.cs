using MediatR;

namespace PlatformService.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;