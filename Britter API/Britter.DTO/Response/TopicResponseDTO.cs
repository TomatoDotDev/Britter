using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.DTO.Response
{
    /// <summary>
    /// Topic response data transfer object.
    /// </summary>
    public class TopicResponseDTO
    {
        /// <summary>
        /// Topic ID.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The topic title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The topic description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The topic creation date.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Whether the post has been edited.
        /// </summary>
        public bool IsEdited { get; set; }

        /// <summary>
        /// The number of upvotes.
        /// </summary>
        public int Upvotes { get; set; }

        /// <summary>
        /// The number of downvotes.
        /// </summary>
        public int Downvotes { get; set; }

        /// <summary>
        /// The number of posts.
        /// </summary>
        public int NumberOfPosts { get; set; }

        /// <summary>
        /// The author of the topic.
        /// </summary>
        public string Author { get; set; }
    }
}
