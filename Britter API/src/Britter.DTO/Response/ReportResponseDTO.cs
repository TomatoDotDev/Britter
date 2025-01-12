namespace Britter.DTO.Response
{
    /// <summary>
    /// Report response data transfer object.
    /// </summary>
    public class ReportResponseDTO
    {
        /// <summary>
        /// The id of the report.
        /// </summary>
        public Guid ReportId { get; set; }

        /// <summary>
        /// The reason for the report.
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// The status of the report.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// The datetime when the report was submitted.
        /// </summary>
        public DateTime SubmittedAt { get; set; }

        /// <summary>
        /// The name of the user who submitted the report.
        /// </summary>
        public string SubmittedBy { get; set; } = string.Empty;


    }
}
