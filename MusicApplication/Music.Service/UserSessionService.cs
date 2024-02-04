using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Music.IService;

namespace MusicService;

public class UserSessionService : IUserSessionService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserSessionService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetCurrentUserName() {
        var http = _httpContextAccessor.HttpContext;
        var userName = http.User?.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.Name);
        return userName?.Value;
    }
    
    public string GetCurrentUserIdToString() {
        var http = _httpContextAccessor.HttpContext;
        var userId = http.User?.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier);
        return userId?.Value;
    }
}