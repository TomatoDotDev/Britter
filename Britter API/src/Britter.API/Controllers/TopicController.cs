using Britter.API.Services;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Britter.DTO.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Britter.API.Controllers
{
    /// <summary>
    /// Controller for topic actions.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowLocalhost3000")]
    public class TopicController : Controller
    {
        private readonly ITopicRepo _repo;
        private readonly IUserUtilityService _userUtility;

        public TopicController(ITopicRepo repo, IUserUtilityService userUtility)
        {
            _repo = repo;
            _userUtility = userUtility;
        }

        /// <summary>
        /// Gets a list of topics based on the search query.
        /// </summary>
        /// <param name="query">The search query.</param>
        /// <returns>A list of users matching search query.</returns>
        [HttpGet]
        public async Task<ActionResult<TopicResponseDTO>> GetTopics([FromForm]TopicQueryDTO query)
        {
            var results = new List<TopicResponseDTO>();
            var topics = await _repo.GetTopicAsync(query);
            foreach (var topic in topics)
            {
                results.Add(ConvertToTopicResponse(topic));
            }

            return Ok(results);
        }

        /// <summary>
        /// Creates a topic.
        /// </summary>
        /// <param name="topicRequest">The topic creation params.</param>
        /// <returns>Status code outlining result of operation.</returns>
        [HttpPost("Create")]
        [Authorize]
        public async Task<ActionResult> CreateTopic(TopicCreateDTO topicRequest)
        {
            var user = await _userUtility.GetUserFromClaimAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var creationDate = DateTime.UtcNow;
            var topic = new Topic()
            {
                Title = topicRequest.Title,
                Description = topicRequest.Description,
                AuthorId = user.Id,
                CreatedAt = creationDate,
                LastUpdatedAt = creationDate,
            };
            await _repo.CreateTopicAsync(topic);

            return Created();
        }

        /// <summary>
        /// Updates a topic.
        /// </summary>
        /// <param name="topicRequest">The topic request params.</param>
        /// <returns></returns>
        [HttpPut("Edit")]
        [Authorize]
        public async Task<ActionResult> UpdateTopic(Guid id, TopicCreateDTO topicRequest)
        {
            var topic = await _repo.GetTopicAsync(new()
            {
                Id = id
            });

            if (!topic.Any())
            {
                return NotFound();
            }

            var topicToUpdate = topic.First();

            var canUpdate = await VerifyUserPermission(topicToUpdate);
            if (!canUpdate)
            {
                return Unauthorized();
            }

            topicToUpdate.Title = topicRequest.Title;
            topicToUpdate.Description = topicRequest.Description;
            topicToUpdate.LastUpdatedAt = DateTime.UtcNow;
            await _repo.UpdateTopicAsync(topicToUpdate);
            return Ok();
        }

        /// <summary>
        /// Deletes a topic for the provided Id.
        /// </summary>
        /// <param name="id">The id of the topic to delete.</param>
        /// <returns>A status code indicating the result of the operation.</returns>
        [HttpDelete("Delete")]
        [Authorize]
        public async Task<ActionResult> DeleteTopic([FromQuery]Guid id)
        {
            var topic = await _repo.GetTopicAsync(new()
            {
                Id = id
            });
            if (!topic.Any())
            {
                return NotFound();
            }

            var topicToDelete = topic.First();
            
            var canDelete = await VerifyUserPermission(topicToDelete);
            if (!canDelete)
            {
                return Unauthorized();
            }

            await _repo.DeleteTopicAsync(topicToDelete);
            return Ok();
        }

        private async Task<bool> VerifyUserPermission(Topic topic)
        {
            var user = await _userUtility.GetUserFromClaimAsync(User);
            if (user == null)
            {
                return false;
            }
            else if ((await _userUtility.VerifyAdminRole(user)) || (topic.AuthorId == user.Id))
            {
                return true;
            }

            return false;
        }

        private TopicResponseDTO ConvertToTopicResponse(Topic topic)
        {
            return new TopicResponseDTO()
            {
                Id = topic.TopicId,
                Title = topic.Title,
                Description = topic.Description,
                CreatedAt = topic.CreatedAt,
                IsEdited = (topic.CreatedAt != topic.LastUpdatedAt),
                NumberOfPosts = topic.Posts.Count,
                Author = topic.Author == null ? "" : topic.Author.UserName,
            };
        }
    }
}
