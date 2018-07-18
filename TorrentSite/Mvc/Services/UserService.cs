using System.Linq;

using SitefinityWebApp.Mvc.Common;
using SitefinityWebApp.Mvc.Services.Contracts;

using Telerik.Sitefinity.Security;

namespace SitefinityWebApp.Mvc.Services
{
    public class UserService : IUserService
    {
        public bool CheckIfUserIsAuthenticated(SitefinityIdentity user)
        {
            return user.IsAuthenticated;
        }

        public bool CheckIfUserIsModerator(SitefinityIdentity user)
        {
            return user.Roles.Any(r => r.Name == Constants.ModeratorRole && r.Provider == Constants.ProviderOfRoles);
        }
    }
}
