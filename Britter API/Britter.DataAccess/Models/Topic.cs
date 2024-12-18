using System.ComponentModel.DataAnnotations;

namespace Britter.Models
{
    /// <summary>
    /// Discussion Topic record.
    /// </summary>
    public class Topic
    {
        /// <summary>
        /// Topic ID.
        /// </summary>
        [Key]
        public Guid TopicId { get; set; }

        /// <summary>
        /// The topic title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The topic description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The user who authored the topic.
        /// </summary>
        public BritterUser Author { get; set; }

        /// <summary>
        /// The date which the topic was created at.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date when the topic was last updated at.
        /// </summary>
        public DateTime? LastUpdatedAt { get; set; }

        /// <summary>
        /// The votes associated with this topic.
        /// </summary>
        public ICollection<Vote> Votes { get; set; }

        /// <summary>
        /// The posts associated with this topic.
        /// </summary>
        public ICollection<Post> Posts { get; set; }
    }
}
