using Microsoft.AspNetCore.Authorization;

namespace Hive.Common.Core.Security.Requirements
{
    public record OnlyOwnerAuthorizationRequirement(bool AllowAdmin = true) : IAuthorizationRequirement { }
}