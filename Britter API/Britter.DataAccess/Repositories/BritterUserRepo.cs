using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using Britter.DTO.Request;
using Microsoft.EntityFrameworkCore;

namespace Britter.DataAccess.Repositories
{
    /// <inheritdoc cref="IBritterUserRepo"/>
    public class BritterUserRepo : IBritterUserRepo
    {
        /// <inheritdoc />
        public async Task<IEnumerable<BritterUser>> GetUserAsync(IQueryable<BritterUser> users, BritterUserSearchQueryDTO query)
        {
            var allUsers = users;

            if (!string.IsNullOrWhiteSpace(query.Email))
            {
                allUsers = allUsers.Where(u => u.Email == query.Email);
            }

            if (!string.IsNullOrWhiteSpace(query.UserName))
            {
                allUsers = allUsers.Where(u => u.UserName == query.UserName);
            }

            allUsers = allUsers.Where(u => (u.LockoutEnd.HasValue == query.IsBlocked));

            if (query.Id.HasValue)
            {
                allUsers = allUsers.Where(u => u.Id == query.Id);
            }

            var pagedUsers = await allUsers
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return pagedUsers;
        }
    }
}
