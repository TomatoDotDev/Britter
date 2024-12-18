using Microsoft.AspNetCore.Identity;

namespace Britter.Models
{
    /// <summary>
    /// User account record.
    /// </summary>
    public class BritterUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Whether or not the user has accepted the code of conduct.
        /// </summary>
        public bool AcceptedCodeOfConduct { get; set; }

        /// <summary>
        /// Date and time when user account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date and time when user account last logged in.
        /// </summary>
        public DateTime? LastLoggedInAt { get; set; }

        /// <summary>
        /// Topics created by user.
        /// </summary>
        public ICollection<Topic> Topics { get; set; }

        /// <summary>
        /// Posts created by user.
        /// </summary>
        public ICollection<Post> Posts { get; set; }

        /// <summary>
        /// All votes made by user.
        /// </summary>
        public ICollection<Vote> Votes { get; set; }

        /// <summary>
        /// All reports made by user.
        /// </summary>
        public ICollection<Report> Reports { get; set; }
    }
}
