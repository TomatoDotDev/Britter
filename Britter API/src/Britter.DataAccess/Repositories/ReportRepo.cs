using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using Britter.DTO.Request;
using Britter.Models.Enums;

namespace Britter.DataAccess.Repositories
{
    /// <inheritdoc cref="IReportRepo"/>
    public class ReportRepo : IReportRepo
    {
        private readonly BritterDBContext _context;

        public ReportRepo(BritterDBContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task CreateReportAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateReportAsync(Report report)
        {
            _context.Reports.Update(report);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Report>> GetReportAsync(ReportQueryDTO query)
        {
            var reports = _context.Reports.AsQueryable();

            if (query.Id.HasValue)
            {
                reports = reports.Where(r => r.ReportId == query.Id);
            }

            if (!string.IsNullOrWhiteSpace(query.SubmittedBy))
            {
                reports = reports.Where(r => r.Reporter.UserName!.Contains(query.SubmittedBy));
            }

            if (!string.IsNullOrWhiteSpace(query.Status) && Enum.TryParse<ReportStatus>(query.Status, out var status))
            {
                reports = reports.Where(r => r.Status == status);
            }

            return reports;
        }
    }
}
