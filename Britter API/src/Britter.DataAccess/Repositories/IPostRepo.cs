using Britter.DataAccess.Models;
using Britter.DTO.Request;

namespace Britter.DataAccess.Repositories
{
    /// <summary>
    /// Post repository access service.
    /// </summary>
    public interface IPostRepo
    {
        /// <summary>
        /// Creates a new post in the db.
        /// </summary>
        /// <param name="Post">The post to create.</param>
        /// <returns>An awaitable task.</returns>
        Task CreatePostAsync(Post Post);

        /// <summary>
        /// Deletes a post from the db.
        /// </summary>
        /// <param name="Post">The post to delete.</param>
        /// <returns></returns>
        Task DeletePostAsync(Post Post);

        /// <summary>
        /// Gets posts that match the filter provided.
        /// </summary>
        /// <param name="query">The query filter.</param>
        /// <returns>A list of posts matching the provided filter.</returns>
        Task<IEnumerable<Post>> GetPostAsync(PostQueryDTO query);

        /// <summary>
        /// Updates a post in the db.
        /// </summary>
        /// <param name="Post">The post to update.</param>
        /// <returns></returns>
        Task UpdatePostAsync(Post Post);
    }
}