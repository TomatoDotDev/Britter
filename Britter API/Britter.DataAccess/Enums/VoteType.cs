using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.Models.Enums
{
    /// <summary>
    /// Vote type.
    /// </summary>
    public enum VoteType : int
    {
        /// <summary>
        /// An upvote.
        /// </summary>
        Upvote = 1,

        /// <summary>
        /// A downvote.
        /// </summary>
        Downvote = -1,
    }
}
