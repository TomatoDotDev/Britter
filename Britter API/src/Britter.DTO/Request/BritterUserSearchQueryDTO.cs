using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Britter.DTO.Request
{
    /// <summary>
    /// Search query DTO for Britter User.
    /// </summary>
    public class BritterUserSearchQueryDTO
    {
        /// <summary>
        /// The email address.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Whether the user is blocked or not.
        /// </summary>
        [DefaultValue(false)]
        public bool IsBlocked { get; set; }

        /// <summary>
        /// The id of the user.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The username of the user.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Gets or sets the size of the page, aka the number of records to return.
        /// </summary>
        [DefaultValue(50)]
        [Range(1, 50)]
        [Required]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the page number to get.
        /// </summary>
        [DefaultValue(1)]
        [Required]
        public int PageNumber { get; set; } = 1;


    }
}
