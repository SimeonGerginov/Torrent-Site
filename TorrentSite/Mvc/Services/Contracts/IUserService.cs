using Telerik.Sitefinity.Security;

namespace SitefinityWebApp.Mvc.Services.Contracts
{
    public interface IUserService
    {
        bool CheckIfUserIsAuthenticated(SitefinityIdentity user);

        bool CheckIfUserIsModerator(SitefinityIdentity user);
    }
}
