using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Britter.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Britter.API.Controllers
{
    /// <summary>
    /// Controller to manage user account actions.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowLocalhost3000")]
    public class UserController : Controller
    {
        private readonly IBritterUserRepo _repo;
        private readonly UserManager<BritterUser> _userManager;

        public UserController(IBritterUserRepo repo, UserManager<BritterUser> userManager) 
        {
            _repo = repo;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets a list of users based on the search query.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns>A list of users matching search query.</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<BritterUserResponseDTO>> GetUsers([FromQuery]BritterUserSearchQueryDTO query)
        {
            var results = new List<BritterUserResponseDTO>();
            var storedUsers = _userManager.Users;
            var users = await _repo.GetUserAsync(storedUsers, query);
            foreach (var user in users)
            {
                results.Add(new BritterUserResponseDTO()
                {
                    UserName = user.UserName,
                    Id = user.Id,
                    Email = user.Email,
                    LoginAttemptsFailed = user.AccessFailedCount,
                    IsLocked = user.LockoutEnd.HasValue
                });
            }
            return Ok(results);
        }

        /// <summary>
        /// Changes the lock status of the provided user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <param name="isLocked">Whether to lock or unlock the account.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpPut("ModifyLockStatus")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ChangeUserLockStatus([FromBody]string userId, [FromQuery]bool isLocked)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                return NotFound();
            }

            DateTimeOffset? lockoutEnd = isLocked ? DateTimeOffset.MaxValue : null;
            var result = await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);

            if (result != IdentityResult.Success)
            {
                return Conflict();
            }

            return Ok();
        }

        /// <summary>
        /// Deletes a user associated with the user id provided.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpDelete("Delete")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteUser([FromBody]Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (result != IdentityResult.Success)
            {
                return Conflict();
            }

            return Ok();
        }

        [HttpGet("IsAdmin")]
        [Authorize]
        public async Task<ActionResult<bool>> IsUserAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return NotFound();
            }

            return Ok(await _userManager.IsInRoleAsync(user, "admin"));
        }
    }
}
