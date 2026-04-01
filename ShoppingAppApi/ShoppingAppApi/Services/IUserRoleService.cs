namespace ShoppingAppApi.Services;

public interface IUserRoleService
{
    bool IsAdmin(HttpContext httpContext);
}
