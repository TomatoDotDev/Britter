using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Britter.DataAccess.Models
{
    /// <summary>
    /// Post class.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// The id for this post.
        /// </summary>
        [Key]
        public Guid PostId { get; set; }

        /// <summary>
        /// The ID of the topic which this post relates to.
        /// </summary>
        public Guid TopicId { get; set; }

        /// <summary>
        /// The ID of the parent post. Can be null.
        /// </summary>
        public Guid? ParentPostId { get; set; }

        /// <summary>
        /// The content of the post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The author of the post's Id.
        /// </summary>
        public Guid AuthorId { get; set; }

        /// <summary>
        /// The date the post was created.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The date the post was last edited.
        /// </summary>
        public DateTime LastEditedAt { get; set; }

        /// <summary>
        /// Whether or not this post was marked for deletion by an admin.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The topic this post is under.
        /// </summary>
        [ForeignKey(nameof(TopicId))]
        public virtual Topic Topic { get; set; }

        /// <summary>
        /// The user this post was authored by.
        /// </summary>
        [ForeignKey(nameof(AuthorId))]
        public virtual BritterUser User { get; set; }

        /// <summary>
        /// The parent of this post.
        /// </summary>
        [ForeignKey(nameof(ParentPostId))]
        public virtual Post? ParentPost { get; set; }

        /// <summary>
        /// List of votes associated with this post.
        /// </summary>
        public virtual ICollection<Vote> Votes { get; set; } = new List<Vote>();

        /// <summary>
        /// List of posts associated with this post.
        /// </summary>
        public virtual ICollection<Post> Replies { get; set; } = new List<Post>();

        /// <summary>
        /// List of reports associated with this post.
        /// </summary>
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
