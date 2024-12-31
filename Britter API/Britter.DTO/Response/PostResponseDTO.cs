using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.DTO.Response
{
    /// <summary>
    /// Post response data transfer object.
    /// </summary>
    public class PostResponseDTO
    {
        /// <summary>
        /// The id of the post.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The content of the post.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The responses to the post.
        /// </summary>
        public List<PostResponseDTO> Responses { get; set; }

        /// <summary>
        /// The author of the post.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// The date the post was created.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Whether or not the post has been edited since creation.
        /// </summary>
        public bool IsEdited { get; set; }

        /// <summary>
        /// Whether or not the post has been deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The number of upvotes the post has.
        /// </summary>
        public int Upvotes { get; set; }

        /// <summary>
        /// The number of downvotes the post has.
        /// </summary>
        public int Downvotes { get; set; }

        /// <summary>
        /// The number of replies the post has.
        /// </summary>
        public int NumberOfReplies { get; set; }
    }
}
