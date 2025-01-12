using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Britter.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Britter.DataAccess.UnitTests.Repositories
{
    public class ReportRepoTests
    {
        [Fact]
        public async Task CreateReportAsync_AddsReport()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new ReportRepo(ctx);
            var report = new Report { ReportId = Guid.NewGuid() };

            // Act
            await repo.CreateReportAsync(report);

            // Assert
            var addedReport = await ctx.Reports.FindAsync(report.ReportId);
            Assert.NotNull(addedReport);
        }

        [Fact]
        public async Task UpdateReportAsync_UpdatesReport()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new ReportRepo(ctx);
            var report = new Report { ReportId = Guid.NewGuid() };
            await ctx.Reports.AddAsync(report);
            await ctx.SaveChangesAsync();

            report.Reason = "Updated reason";

            // Act
            await repo.UpdateReportAsync(report);

            // Assert
            var updatedReport = await ctx.Reports.FindAsync(report.ReportId);
            Assert.Equal("Updated reason", updatedReport.Reason);
        }

        [Fact]
        public async Task GetReportAsync_FiltersById()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new ReportRepo(ctx);
            var reportId = Guid.NewGuid();
            var reports = new List<Report>
            {
                new Report { ReportId = reportId },
                new Report { ReportId = Guid.NewGuid() }
            };

            await ctx.Reports.AddRangeAsync(reports);
            await ctx.SaveChangesAsync();

            var query = new ReportQueryDTO { Id = reportId };

            // Act
            var result = await repo.GetReportAsync(query);

            // Assert
            Assert.Single(result);
            Assert.Equal(reportId, result.First().ReportId);
        }

        [Fact]
        public async Task GetReportAsync_FiltersBySubmittedBy()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new ReportRepo(ctx);
            var submittedBy = "testuser";
            var reports = new List<Report>
            {
                new Report { Reporter = new BritterUser { UserName = submittedBy } },
                new Report { Reporter = new BritterUser { UserName = "otheruser" } }
            };

            await ctx.Reports.AddRangeAsync(reports);
            await ctx.SaveChangesAsync();

            var query = new ReportQueryDTO { SubmittedBy = submittedBy };

            // Act
            var result = await repo.GetReportAsync(query);

            // Assert
            Assert.Single(result);
            Assert.Equal(submittedBy, result.First().Reporter.UserName);
        }

        [Fact]
        public async Task GetReportAsync_FiltersByStatus()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new ReportRepo(ctx);
            var status = ReportStatus.Resolved;
            var reports = new List<Report>
            {
                new Report { Status = status },
                new Report { Status = ReportStatus.Open }
            };

            await ctx.Reports.AddRangeAsync(reports);
            await ctx.SaveChangesAsync();

            var query = new ReportQueryDTO { Status = status.ToString() };

            // Act
            var result = await repo.GetReportAsync(query);

            // Assert
            Assert.Single(result);
            Assert.Equal(status, result.First().Status);
        }
    }
}