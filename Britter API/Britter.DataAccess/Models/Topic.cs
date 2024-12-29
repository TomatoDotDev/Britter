using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Britter.DataAccess.Models
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
        /// The author's ID.
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// The user who authored the topic.
        /// </summary>
        [ForeignKey(nameof(AuthorId))]
        public virtual BritterUser Author { get; set; }

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
        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();

        /// <summary>
        /// The posts associated with this topic.
        /// </summary>
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        /// <summary>
        /// Reports associated with the topic.
        /// </summary>
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
