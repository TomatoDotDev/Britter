using Microsoft.AspNetCore.Identity;

namespace Britter.DataAccess.Models
{
    /// <summary>
    /// User account record. Inherits from <see cref="IdentityUser"/>
    /// </summary>
    public class BritterUser : IdentityUser<Guid>
    {
        /// <summary>
        /// Topics created by user.
        /// </summary>
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();

        /// <summary>
        /// Posts created by user.
        /// </summary>
        public ICollection<Post> Posts { get; set; } = new List<Post>();

        /// <summary>
        /// All votes made by user.
        /// </summary>
        public ICollection<Vote> Votes { get; set; } = new List<Vote>();

        /// <summary>
        /// All reports made by user.
        /// </summary>
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
