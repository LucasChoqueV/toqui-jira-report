using TJR.Common.Results;
using MediatR;

namespace TJR.Common.Application.CQRS
{
    public abstract class CommandHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IResult
    {
        public CommandHandlerBase() { }

        protected TRequest CommandRequest { get; private set; }
        protected CancellationToken CancellationToken { get; private set; }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            CommandRequest = request;
            CancellationToken = cancellationToken;
            return await HandleAsync().ConfigureAwait(false);
        }

        protected abstract Task<TResponse> HandleAsync();
    }
}
