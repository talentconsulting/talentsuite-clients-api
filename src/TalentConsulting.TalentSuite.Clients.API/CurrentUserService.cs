using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using TalentConsulting.TalentSuite.Clients.Core;

namespace TalentConsulting.TalentSuite.Clients.API;

[ExcludeFromCodeCoverage]
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
}

