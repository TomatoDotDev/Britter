using Britter.DataAccess.Context;
using Britter.DataAccess.Models;

namespace Britter.DataAccess.Repositories
{
    /// <inheritdoc cref="IVoteRepo"/>
    public class VoteRepo : IVoteRepo
    {
        private readonly BritterDBContext _context;

        public VoteRepo(BritterDBContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task CreateVoteAsync(Vote vote)
        {
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteVoteAsync(Vote vote)
        {
            _context.Votes.Remove(vote);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Vote?> GetVoteAsync(Guid postId, Guid userId)
        {
            return await _context.Votes.FindAsync(postId, userId);
        }
    }
}
