using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using Britter.DTO.Request;

namespace Britter.DataAccess.Repositories
{
    /// <inheritdoc cref="ITopicRepo"/>
    public class TopicRepo : ITopicRepo
    {
        private readonly BritterDBContext _context;

        public TopicRepo(BritterDBContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Topic>> GetTopicAsync(TopicQueryDTO query)
        {
            var topics = _context.Topics.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                topics = topics.Where(t => t.Title.ToLower().Contains(query.Title.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(query.Description))
            {
                topics = topics.Where(t => t.Description.ToLower().Contains(query.Description.ToLower()));
            }

            if (query.Id.HasValue)
            {
                topics = topics.Where(t => t.TopicId == query.Id);
            }

            return topics;
        }

        /// <inheritdoc />
        public async Task CreateTopicAsync(Topic topic)
        {
            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateTopicAsync(Topic topic)
        {
            _context.Topics.Update(topic);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteTopicAsync(Topic topic)
        {
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
        }
    }
}
