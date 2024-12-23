namespace Britter.DTO.Response
{
    /// <summary>
    /// Response data transfer object for a britter user.
    /// </summary>
    public class BritterUserResponseDTO
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The username of the user.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The number of failed login attempts.
        /// </summary>
        public int LoginAttemptsFailed { get; set; }

        /// <summary>
        /// Whether the user account is locked.
        /// </summary>
        public bool IsLocked { get; set; }
    }
}
