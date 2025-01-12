using Britter.DataAccess.Models;
using Britter.DTO.Request;

namespace Britter.DataAccess.Repositories
{
    /// <summary>
    /// The report repository for accessing the db.
    /// </summary>
    public interface IReportRepo
    {
        /// <summary>
        /// Create a new report.
        /// </summary>
        /// <param name="report">The report to create.</param>
        /// <returns>An awaitable task.</returns>
        Task CreateReportAsync(Report report);

        /// <summary>
        /// Get a list of reports based on the provided query.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>A list of reports matching the report.</returns>
        Task<IEnumerable<Report>> GetReportAsync(ReportQueryDTO query);

        /// <summary>
        /// Update a report.
        /// </summary>
        /// <param name="report">The report to update.</param>
        /// <returns>An awaitable task.</returns>
        Task UpdateReportAsync(Report report);
    }
}