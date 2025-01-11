using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.DTO.Request
{
    /// <summary>
    /// DTO for querying report data.
    /// </summary>
    public class ReportQueryDTO
    {
        /// <summary>
        /// The id of the report.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// The status of the report.
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// The author of the report.
        /// </summary>
        public string SubmittedBy { get; set; } = string.Empty;


    }
}
