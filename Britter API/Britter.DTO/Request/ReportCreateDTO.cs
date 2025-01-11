namespace Britter.DTO.Request
{
    /// <summary>
    /// A DTO for creating a report.
    /// </summary>
    public class ReportCreateDTO
    {
        /// <summary>
        /// The id of the post that this report relates to.
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// The reason for the report.
        /// </summary>
        public string Reason { get; set; } = string.Empty;
    }
}
