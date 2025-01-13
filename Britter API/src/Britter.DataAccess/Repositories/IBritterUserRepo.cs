using Britter.DataAccess.Models;
using Britter.DTO.Request;

namespace Britter.DataAccess.Repositories
{
    /// <summary>
    /// Britter User table data access repository.
    /// </summary>
    public interface IBritterUserRepo
    {
        /// <summary>
        /// Gets a user based on the search query parameters.
        /// </summary>
        /// <param name="query">The query parameters to use.</param>
        /// <returns>A list of users matching the specified parameters.</returns>
        Task<IEnumerable<BritterUser>> GetUserAsync(IQueryable<BritterUser> users, BritterUserSearchQueryDTO query);
    }
}