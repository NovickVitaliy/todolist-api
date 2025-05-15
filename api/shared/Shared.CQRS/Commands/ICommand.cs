using MediatR;

namespace Shared.CQRS.Commands;

public interface ICommand<out TResponse> : IRequest<TResponse>;

public interface ICommand : IRequest<Unit>;