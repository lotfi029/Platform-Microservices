using MediatR;

namespace PlatformService.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, Result<TResult>>
    where TQuery : IQuery<TResult>;