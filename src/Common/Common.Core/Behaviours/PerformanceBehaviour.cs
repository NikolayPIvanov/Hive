using Hive.Common.Core.Interfaces;

namespace Hive.Common.Core.Behaviours
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    
    using MediatR;

    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public PerformanceBehaviour(
            ICurrentUserService currentUserService,
            IIdentityService identityService,
            ILogger<TRequest> logger)
        {
            _timer = new Stopwatch();

            _currentUserService = currentUserService;
            _identityService = identityService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds <= 500) return response;
            var requestName = typeof(TRequest).Name;
            var userId = string.Empty;
            var userName = string.Empty;

            _logger.LogWarning("Hive Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                requestName, elapsedMilliseconds, userId, userName, request);

            return response;
        }
    }
}
