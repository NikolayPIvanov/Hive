using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Hive.Common.Core.Security.Requirements
{
    public record OnlyOwnerAuthorizationRequirement(IEnumerable<string> SuperRoles) : IAuthorizationRequirement { }
}