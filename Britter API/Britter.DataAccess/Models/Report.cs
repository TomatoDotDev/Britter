using Britter.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Britter.Models
{
    /// <summary>
    /// A report record.
    /// </summary>
    public class Report
    {
        /// <summary>
        /// The report id.
        /// </summary>
        [Key]
        public Guid ReportId { get; set; }

        /// <summary>
        /// The id of the post that this report relates to.
        /// </summary>
        public Guid PostId { get; set; }

        /// <summary>
        /// The id of the author of this report.
        /// </summary>
        public Guid ReportedById { get; set; }

        /// <summary>
        /// The reason for the report.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// The date the report was created.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// The current status of the report.
        /// </summary>
        public ReportStatus Status { get; set; }

        /// <summary>
        /// The related post.
        /// </summary>
        [ForeignKey(nameof(PostId))]
        public Post Post { get; set; }

        /// <summary>
        /// The author of the post.
        /// </summary>
        [ForeignKey(nameof(ReportedById))]
        public BritterUser Reporter { get; set; }
    }
}
