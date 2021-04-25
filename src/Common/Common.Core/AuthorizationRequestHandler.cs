using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.SeedWork;
using Microsoft.AspNetCore.Authorization;

namespace Hive.Common.Core
{
    public abstract class AuthorizationRequestHandler<TEntity> where TEntity : Entity
    {
        protected readonly ICurrentUserService CurrentUserService;
        protected readonly IAuthorizationService AuthorizationService;

        protected AuthorizationRequestHandler(ICurrentUserService currentUserService, IAuthorizationService authorizationService)
        {
            CurrentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            AuthorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        }

        protected virtual async Task<AuthorizationResult[]> AuthorizeAsync(TEntity entity,IEnumerable<string> policies)
        {
            var authorizationTasks = policies
                .Select(policy => AuthorizationService.AuthorizeAsync(CurrentUserService.User, entity, policy));

            var results = await Task.WhenAll(authorizationTasks);

            return results;
        }

    }
}