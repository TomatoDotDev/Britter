using Britter.API.Services;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Britter.DTO.Response;
using Britter.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Britter.API.Controllers
{
    /// <summary>
    /// Controller for post actions.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowLocalhost3000")]
    public class PostController : Controller
    {
        private readonly IPostRepo _repo;
        private readonly IUserUtilityService _userUtility;

        public PostController(IPostRepo repo, IUserUtilityService userUtility)
        {
            _repo = repo;
            _userUtility = userUtility;
        }

        /// <summary>
        /// Gets all posts based on the provided filter.
        /// </summary>
        /// <param name="query">The query filter to use.</param>
        /// <returns>A list of posts matching the filter.</returns>
        [HttpPost]
        public async Task<ActionResult<List<PostResponseDTO>>> GetPostAsync(PostQueryDTO query)
        {
            var post = await _repo.GetPostAsync(query);
            var results = new List<PostResponseDTO>();
            foreach (var p in post)
            {
                results.Add(ConvertToPostResponse(p));
            }

            return Ok(results);
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="post">The post details.</param>
        /// <returns>A status code indicating outcome of the operation.</returns>
        [HttpPost("Create")]
        [Authorize]
        public async Task<ActionResult> CreatePostAsync(PostCreateDTO post)
        {
            var user = await _userUtility.GetUserFromClaimAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var creationDate = DateTime.UtcNow;
            var postRecord = new Post()
            {
                Content = post.Content,
                AuthorId = user.Id,
                ParentPostId = post.ParentPostId,
                TopicId = post.TopicId,
                CreationDate = creationDate,
                LastEditedAt = creationDate,
            };

            await _repo.CreatePostAsync(postRecord);

            return Created();
        }

        /// <summary>
        /// Edits an existing post.
        /// </summary>
        /// <param name="id">The id of the post to edit.</param>
        /// <param name="newContent">New post content.</param>
        /// <returns>A status code indicating the outcome of the operation.</returns>
        [HttpPut("Edit")]
        [Authorize]
        public async Task<ActionResult> UpdatePostAsync(Guid id, [FromBody]string newContent)
        {
            var postRecord = await _repo.GetPostAsync(new PostQueryDTO { Id = id, ShowOnlyTopLevel = false });
            if (!postRecord.Any())
            {
                return NotFound();
            }

            var topicToUpdate = postRecord.First();

            var canUpdate = await VerifyUserPermission(topicToUpdate);
            if (!canUpdate)
            {
                return Unauthorized();
            }

            topicToUpdate.Content = newContent;
            topicToUpdate.LastEditedAt = DateTime.UtcNow;

            await _repo.UpdatePostAsync(topicToUpdate);
            return Ok();
        }

        /// <summary>
        /// Deletes a post.
        /// </summary>
        /// <param name="id">The id of the post to delete.</param>
        /// <returns>A status code indicating the outcome of the operation.</returns>
        [HttpDelete("Delete")]
        [Authorize]
        public async Task<ActionResult> DeletePostAsync(Guid id)
        {
            var posts = await _repo.GetPostAsync(new()
            {
                Id = id,
                ShowOnlyTopLevel = false
            });

            if (!posts.Any())
            {
                return NotFound();
            }

            var postToDelete = posts.First();

            var canDelete = await VerifyUserPermission(postToDelete);
            if (!canDelete)
            {
                return Unauthorized();
            }

            // mark the post as deleted.
            postToDelete.IsDeleted = true;

            await _repo.UpdatePostAsync(postToDelete);
            return Ok();
        }


        private PostResponseDTO ConvertToPostResponse(Post post)
        {
            return new PostResponseDTO
            {
                Id = post.PostId,
                Content = post.Content,
                Author = post.User == null ? "" : post.User.UserName,
                CreationDate = post.CreationDate,
                IsEdited = post.CreationDate != post.LastEditedAt,
                IsDeleted = post.IsDeleted,
                Upvotes = post.Votes.Count(v => v.Value == VoteType.Upvote),
                Downvotes = post.Votes.Count(v => v.Value == VoteType.Downvote),
                NumberOfReplies = post.Replies.Count,
                Responses = post.Replies.Select(r => ConvertToPostResponse(r)).ToList()
            };
        }

        private async Task<bool> VerifyUserPermission(Post post)
        {
            var user = await _userUtility.GetUserFromClaimAsync(User);
            if (user == null)
            {
                return false;
            }
            else if ((await _userUtility.VerifyAdminRole(user)) || (post.AuthorId == user.Id))
            {
                return true;
            }
            return false;
        }

    }
}
