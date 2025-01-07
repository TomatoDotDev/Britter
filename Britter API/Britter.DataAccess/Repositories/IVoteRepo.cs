using Britter.DataAccess.Models;

namespace Britter.DataAccess.Repositories
{
    /// <summary>
    /// A repository for managing votes.
    /// </summary>
    public interface IVoteRepo
    {
        /// <summary>
        /// Creates a new vote record.
        /// </summary>
        /// <param name="vote">The vote to create.</param>
        /// <returns>An awaitable task.</returns>
        Task CreateVoteAsync(Vote vote);

        /// <summary>
        /// Deletes a vote record.
        /// </summary>
        /// <param name="vote">The vote to delete.</param>
        /// <returns>An awaitable task.</returns>
        Task DeleteVoteAsync(Vote vote);

        /// <summary>
        /// Gets a vote record.
        /// </summary>
        /// <param name="postOrTopicId">The post or topic id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>A post if found, else returns null.</returns>
        Task<Vote?> GetPostAsync(Guid postOrTopicId, Guid userId);
    }
}