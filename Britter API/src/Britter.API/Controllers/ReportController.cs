using Britter.API.Services;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Britter.DTO.Response;
using Britter.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Britter.API.Controllers
{
    /// <summary>
    /// Controller for report actions.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        /// <summary>
        /// The report repository.
        /// </summary>
        private readonly IReportRepo _repo;

        /// <summary>
        /// The user utility service.
        /// </summary>
        private readonly IUserUtilityService _userUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportController"/> class.
        /// </summary>
        /// <param name="repo">The report repository access service.</param>
        /// <param name="userUtility">The user utility service.</param>
        public ReportController(IReportRepo repo, IUserUtilityService userUtility)
        {
            _repo = repo;
            _userUtility = userUtility;
        }

        /// <summary>
        /// Gets all reports based on the provided filter.
        /// </summary>
        /// <param name="query">The query filter to use.</param>
        /// <returns>A list of reports matching the filter.</returns>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<ReportResponseDTO>>> GetReportAsync([FromForm] ReportQueryDTO query)
        {
            var reports = await _repo.GetReportAsync(query);
            var results = new List<ReportResponseDTO>();
            foreach (var report in reports)
            {
                results.Add(ConvertToReportResponse(report));
            }

            return Ok(results);
        }

        /// <summary>
        /// Creates a new report.
        /// </summary>
        /// <param name="report">The report details.</param>
        /// <returns>A status code indicating outcome of the operation.</returns>
        [HttpPost("Create")]
        [Authorize]
        public async Task<ActionResult> CreateReportAsync(ReportCreateDTO report)
        {
            var user = await _userUtility.GetUserFromClaimAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var creationDate = DateTime.UtcNow;
            var reportRecord = new Report()
            {
                PostId = report.PostId,
                ReportedById = user.Id,
                Status = ReportStatus.Open,
                CreationDate = creationDate,
                Reason = report.Reason,
            };

            await _repo.CreateReportAsync(reportRecord);

            return Created();
        }

        /// <summary>
        /// Edits an existing report status.
        /// </summary>
        /// <param name="id">The id of the report to edit.</param>
        /// <param name="status">The new report status information.</param>
        /// <returns>A status code indicating the outcome of the operation.</returns>
        [HttpPut("Edit")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> UpdateReportStatusAsync(Guid id, string status)
        {
            if (!Enum.TryParse<ReportStatus>(status, out var reportStatus))
            {
                return BadRequest();
            }

            var reportRecord = await _repo.GetReportAsync(new ReportQueryDTO { Id = id });
            if (!reportRecord.Any())
            {
                return NotFound();
            }

            var reportToUpdate = reportRecord.First();

            reportToUpdate.Status = reportStatus;

            await _repo.UpdateReportAsync(reportToUpdate);
            return Ok();
        }

        /// <summary>
        /// Converts a report into a response DTO.
        /// </summary>
        /// <param name="report">The report to convert.</param>
        /// <returns>A report response DTO object.</returns>
        private ReportResponseDTO ConvertToReportResponse(Report report)
        {
            return new ReportResponseDTO
            {
                ReportId = report.ReportId,
                Reason = report.Reason,
                Status = report.Status.ToString(),
                SubmittedAt = report.CreationDate,
                SubmittedBy = report.Reporter == null ? "" : report.Reporter.UserName,
            };
        }
    }
}
