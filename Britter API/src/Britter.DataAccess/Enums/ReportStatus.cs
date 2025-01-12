using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.Models.Enums
{
    /// <summary>
    /// The report status.
    /// </summary>
    public enum ReportStatus
    {
        /// <summary>
        /// Report is still open.
        /// </summary>
        Open,

        /// <summary>
        /// Report is reviewed.
        /// </summary>
        Reviewed,

        /// <summary>
        /// Report is resolved.
        /// </summary>
        Resolved
    }
}
