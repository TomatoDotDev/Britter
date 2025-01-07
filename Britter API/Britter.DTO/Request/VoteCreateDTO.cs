using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.DTO.Request
{
    /// <summary>
    /// DTO for creating a vote.
    /// </summary>
    public class VoteCreateDTO
    {
        /// <summary>
        /// The id of the post or topic to vote on.
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// The type of vote. +1 for upvote, -1 for downvote.
        /// </summary>
        public int VoteType { get; set; }
    }
}
