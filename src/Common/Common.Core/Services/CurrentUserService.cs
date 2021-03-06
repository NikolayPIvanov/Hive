﻿using System.Security.Claims;
using Hive.Common.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Hive.Common.Core.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

    }
}