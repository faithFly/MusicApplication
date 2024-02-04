namespace Music.IService;

public interface IUserSessionService
{
    string GetCurrentUserName();
    string GetCurrentUserIdToString();
}