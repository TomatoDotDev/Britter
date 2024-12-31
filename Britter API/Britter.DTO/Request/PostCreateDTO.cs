using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.DTO.Request
{
    /// <summary>
    /// Post creation data transfer object.
    /// </summary>
    public class PostCreateDTO
    {
        /// <summary>
        /// Parent post id.
        /// </summary>
        public Guid? ParentPostId { get; set; }

        /// <summary>
        /// Gets or sets the content of the post.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the topic Id.
        /// </summary>
        public Guid TopicId { get; set; }
    }
}
