using Britter.API.Controllers;
using Britter.API.Services;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Britter.DTO.Response;
using Britter.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Britter.API.UnitTests.Controllers
{
    public class ReportControllerTests
    {
        private readonly Mock<IReportRepo> _mockRepo;
        private readonly Mock<IUserUtilityService> _mockUserUtility;
        private readonly ReportController _controller;

        public ReportControllerTests()
        {
            _mockRepo = new Mock<IReportRepo>();
            _mockUserUtility = new Mock<IUserUtilityService>();
            _controller = new ReportController(_mockRepo.Object, _mockUserUtility.Object);
        }

        [Fact]
        public async Task GetReportAsync_ReturnsReports()
        {
            // Arrange
            var reports = new List<Report>
            {
                new Report { ReportId = Guid.NewGuid(), Reason = "Test reason" }
            };
            _mockRepo.Setup(repo => repo.GetReportAsync(It.IsAny<ReportQueryDTO>())).ReturnsAsync(reports);

            // Act
            var result = await _controller.GetReportAsync(new ReportQueryDTO());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ReportResponseDTO>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal("Test reason", returnValue.First().Reason);
        }

        [Fact]
        public async Task CreateReportAsync_ReturnsCreated_WhenUserIsAuthenticated()
        {
            // Arrange
            var user = new BritterUser { Id = Guid.NewGuid() };
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var reportCreateDto = new ReportCreateDTO { PostId = Guid.NewGuid(), Reason = "Test reason" };

            // Act
            var result = await _controller.CreateReportAsync(reportCreateDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            _mockRepo.Verify(repo => repo.CreateReportAsync(It.IsAny<Report>()), Times.Once);
        }

        [Fact]
        public async Task CreateReportAsync_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((BritterUser?)null);

            var reportCreateDto = new ReportCreateDTO { PostId = Guid.NewGuid(), Reason = "Test reason" };

            // Act
            var result = await _controller.CreateReportAsync(reportCreateDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
            _mockRepo.Verify(repo => repo.CreateReportAsync(It.IsAny<Report>()), Times.Never);
        }

        [Fact]
        public async Task UpdateReportStatusAsync_ReturnsOk_WhenReportStatusIsUpdated()
        {
            // Arrange
            var reportId = Guid.NewGuid();
            var report = new Report { ReportId = reportId, Status = ReportStatus.Open };
            _mockRepo.Setup(repo => repo.GetReportAsync(It.IsAny<ReportQueryDTO>())).ReturnsAsync(new List<Report> { report });

            // Act
            var result = await _controller.UpdateReportStatusAsync(reportId, "Reviewed");

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockRepo.Verify(repo => repo.UpdateReportAsync(It.IsAny<Report>()), Times.Once);
        }

        [Fact]
        public async Task UpdateReportStatusAsync_ReturnsBadRequest_WhenStatusIsInvalid()
        {
            // Arrange
            var reportId = Guid.NewGuid();

            // Act
            var result = await _controller.UpdateReportStatusAsync(reportId, "InvalidStatus");

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            _mockRepo.Verify(repo => repo.UpdateReportAsync(It.IsAny<Report>()), Times.Never);
        }

        [Fact]
        public async Task UpdateReportStatusAsync_ReturnsNotFound_WhenReportDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetReportAsync(It.IsAny<ReportQueryDTO>())).ReturnsAsync(new List<Report>());

            // Act
            var result = await _controller.UpdateReportStatusAsync(Guid.NewGuid(), "Reviewed");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            _mockRepo.Verify(repo => repo.UpdateReportAsync(It.IsAny<Report>()), Times.Never);
        }
    }
}


