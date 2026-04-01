namespace ShoppingAppApi.Services;

public class UserRoleService : IUserRoleService
{
    private const string RoleHeaderName = "X-User-Role";
    private const string AdminRoleValue = "admin";

    public bool IsAdmin(HttpContext httpContext)
    {
        bool hasUserRoleHeader = httpContext.Request.Headers
            .TryGetValue(RoleHeaderName, out var roleValues);

        if (!hasUserRoleHeader)
        {
            return false;
        }

        bool isAdmin = roleValues.Any(value => string.Equals(
            value,
            AdminRoleValue,
            StringComparison.OrdinalIgnoreCase));

        return isAdmin;
    }
}
