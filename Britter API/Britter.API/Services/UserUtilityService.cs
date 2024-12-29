using System.Security.Claims;
using System.Threading.Tasks;
using Britter.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Net.Http.Headers;

namespace Britter.API.Services
{
    /// <inheritdoc cref="IUserUtilityService" />
    public class UserUtilityService : IUserUtilityService
    {
        private readonly UserManager<BritterUser> _userManager;

        public UserUtilityService(UserManager<BritterUser> userManager)
        {
            _userManager = userManager;
        }

        /// <inheritdoc />
        public async Task<BritterUser?> GetUserFromClaimAsync(ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return null;
            }

            return await _userManager.FindByIdAsync(userId);
        }

        /// <inheritdoc />
        public Task<bool> VerifyAdminRole(BritterUser user)
        {
            return _userManager.IsInRoleAsync(user, "admin");
        }
    }
}
