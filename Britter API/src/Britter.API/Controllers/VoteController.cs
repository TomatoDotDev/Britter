using Britter.API.Services;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Britter.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Britter.API.Controllers
{
    /// <summary>
    /// Controller for vote actions.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowLocalhost3000")]
    public class VoteController : Controller
    {
        private readonly IVoteRepo _repo;
        private readonly IUserUtilityService _userUtility;

        public VoteController(IVoteRepo repo, IUserUtilityService userUtility)
        {
            _repo = repo;
            _userUtility = userUtility;
        }

        /// <summary>
        /// Creates a new vote.
        /// </summary>
        /// <param name="vote">The vote details.</param>
        /// <returns>A status code indicating outcome of the operation.</returns>
        [HttpPost("Create")]
        [Authorize]
        public async Task<ActionResult> CreateVoteAsync(VoteCreateDTO vote)
        {
            var user = await _userUtility.GetUserFromClaimAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var voteParse = Enum.TryParse<VoteType>(vote.VoteType.ToString(), out var voteType);
            if (!voteParse)
            {
                return BadRequest("Invalid vote type.");
            }

            var voteRecord = new Vote()
            {
                PostId = vote.PostId,
                UserId = user.Id,
                Value = voteType,
                CreationDate = DateTime.UtcNow,
            };

            await _repo.CreateVoteAsync(voteRecord);
            return Created();
        }

        /// <summary>
        /// Deletes a vote.
        /// </summary>
        /// <param name="TopicOrPostId">The id of the post or topic.</param>
        /// <returns>A status code indicating outcome of the operation.</returns>
        [HttpDelete("Delete")]
        [Authorize]
        public async Task<ActionResult> DeleteVoteAsync(Guid postId)
        {
            var user = await _userUtility.GetUserFromClaimAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var vote = await _repo.GetVoteAsync(postId, user.Id);

            if (vote == null)
            {
                return NotFound();
            }

            await _repo.DeleteVoteAsync(vote);
            return Ok();
        }
    }
}
