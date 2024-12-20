using Britter.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Britter.DataAccess.Models
{
    /// <summary>
    /// A vote record.
    /// </summary>
    [PrimaryKey(nameof(PostOrTopicId), nameof(UserId))]
    public class Vote
    {
        /// <summary>
        /// The id of the vote record.
        /// </summary>
        public Guid PostOrTopicId { get; set; }

        /// <summary>
        /// The id of the user who made the vote.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The type of vote.
        /// </summary>
        public VoteType Value { get; set; }

        /// <summary>
        /// The date when the vote was made.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The user who made the vote.
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public BritterUser User { get; set; }

        /// <summary>
        /// The topic which the vote relates to. can be null.
        /// </summary>
        [ForeignKey(nameof(PostOrTopicId))]
        public Topic? Topic { get; set; }

        /// <summary>
        /// The post which the vote relates to. can be null.
        /// </summary>
        [ForeignKey(nameof(PostOrTopicId))]
        public Post? Post { get; set; }
    }
}
