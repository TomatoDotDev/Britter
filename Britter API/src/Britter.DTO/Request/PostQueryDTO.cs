using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.DTO.Request
{
    /// <summary>
    /// DTO for querying posts.
    /// </summary>
    public class PostQueryDTO
    {
        /// <summary>
        /// The id of the post.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The id of the parent topic.
        /// </summary>
        public Guid? TopicId { get; set; }

        /// <summary>
        /// Whether or not to show posts that have been marked for deletion by administrator. Defaults to false.
        /// </summary>
        public bool ShowDeletedPosts { get; set; } = false;

        /// <summary>
        /// The author of the post.
        /// </summary>
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// The content of the post. Note that this will be checked as a partial match.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Whether or not to show only top level posts. Defaults to true.
        /// </summary>
        public bool ShowOnlyTopLevel { get; set; } = true;
    }
}
