using System.Security.Claims;
using System.Threading.Tasks;
using Britter.DataAccess.Models;

namespace Britter.API.Services
{
    /// <summary>
    /// User manager utility functions.
    /// </summary>
    public interface IUserUtilityService
    {
        /// <summary>
        /// Get the user from the claims principal.
        /// </summary>
        /// <param name="user">User claims.</param>
        /// <returns>A <see cref="BritterUser"/> if found.</returns>
        Task<BritterUser?> GetUserFromClaimAsync(ClaimsPrincipal user);

        /// <summary>
        /// Verify if the user has the admin role.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>A bool indicating if user is an administrator or not.</returns>
        Task<bool> VerifyAdminRole(BritterUser user);
    }
}
