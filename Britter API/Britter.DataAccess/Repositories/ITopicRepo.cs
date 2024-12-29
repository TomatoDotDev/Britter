using Britter.DataAccess.Models;
using Britter.DTO.Request;

namespace Britter.DataAccess.Repositories
{
    public interface ITopicRepo
    {
        /// <summary>
        /// Creates a topic and adds to db.
        /// </summary>
        /// <param name="topic">The topic to create.</param>
        /// <returns>An awaitable task.</returns>
        Task CreateTopicAsync(Topic topic);

        /// <summary>
        /// Deletes a topic from the db.
        /// </summary>
        /// <param name="topic">The topic to delete.</param>
        /// <returns>An awaitable task.</returns>
        Task DeleteTopicAsync(Topic topic);

        /// <summary>
        /// Gets topics based on the provided filter.
        /// </summary>
        /// <param name="query">The filter to use.</param>
        /// <returns>A list of topics matching the filter.</returns>
        Task<IEnumerable<Topic>> GetTopicAsync(TopicQueryDTO query);

        /// <summary>
        /// Updates a provided topic.
        /// </summary>
        /// <remarks>
        /// Ensure that the id is of the record to update, all other fields can be modified.
        /// </remarks>
        /// <param name="topic">The topic to update.</param>
        /// <returns></returns>
        Task UpdateTopicAsync(Topic topic);
    }
}