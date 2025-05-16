using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shared.CQRS.PipelineBehaviours;

public class LoggingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;
    
    public LoggingPipelineBehaviour(ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing '{RequestType}' request", typeof(TRequest).Name);
        var stopwatch = Stopwatch.StartNew();
        var response = await next(cancellationToken);
        var elapsedTime = stopwatch.ElapsedMilliseconds;
        _logger.LogInformation("Processing '{RequestType}' request took {ProcessingTime} ms", typeof(TRequest).Name, elapsedTime);
        return response;
    }
}