namespace Common.Api.Services;

public interface ICurrentUserService
{
    public int CurrentUserId();
    public string CurrentUserName();
    public string[] CurrentUserRoles();
}