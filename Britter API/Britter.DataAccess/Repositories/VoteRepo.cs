using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<Vote?> GetPostAsync(Guid postOrTopicId, Guid userId)
        {
            return await _context.Votes.FindAsync(postOrTopicId, userId);
        }
    }
}
