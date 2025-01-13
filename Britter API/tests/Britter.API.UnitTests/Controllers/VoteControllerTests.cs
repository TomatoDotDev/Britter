using Britter.API.Controllers;
using Britter.API.Services;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Britter.API.UnitTests.Controllers
{
    public class VoteControllerTests
    {
        private readonly Mock<IVoteRepo> _mockRepo;
        private readonly Mock<IUserUtilityService> _mockUserUtility;
        private readonly VoteController _controller;

        public VoteControllerTests()
        {
            _mockRepo = new Mock<IVoteRepo>();
            _mockUserUtility = new Mock<IUserUtilityService>();
            _controller = new VoteController(_mockRepo.Object, _mockUserUtility.Object);
        }

        [Fact]
        public async Task CreateVoteAsync_ReturnsCreated_WhenUserIsAuthenticated()
        {
            // Arrange
            var user = new BritterUser { Id = Guid.NewGuid() };
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var voteCreateDto = new VoteCreateDTO { PostId = Guid.NewGuid(), VoteType = 1 };

            // Act
            var result = await _controller.CreateVoteAsync(voteCreateDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            _mockRepo.Verify(repo => repo.CreateVoteAsync(It.IsAny<Vote>()), Times.Once);
        }

        [Fact]
        public async Task CreateVoteAsync_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((BritterUser?)null);

            var voteCreateDto = new VoteCreateDTO { PostId = Guid.NewGuid(), VoteType = 1 };

            // Act
            var result = await _controller.CreateVoteAsync(voteCreateDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
            _mockRepo.Verify(repo => repo.CreateVoteAsync(It.IsAny<Vote>()), Times.Never);
        }

        [Fact]
        public async Task DeleteVoteAsync_ReturnsOk_WhenVoteIsDeleted()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var user = new BritterUser { Id = Guid.NewGuid() };
            var vote = new Vote { PostId = postId, UserId = user.Id };
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _mockRepo.Setup(repo => repo.GetVoteAsync(postId, user.Id)).ReturnsAsync(vote);

            // Act
            var result = await _controller.DeleteVoteAsync(postId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockRepo.Verify(repo => repo.DeleteVoteAsync(It.IsAny<Vote>()), Times.Once);
        }

        [Fact]
        public async Task DeleteVoteAsync_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((BritterUser?)null);

            // Act
            var result = await _controller.DeleteVoteAsync(Guid.NewGuid());

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
            _mockRepo.Verify(repo => repo.DeleteVoteAsync(It.IsAny<Vote>()), Times.Never);
        }

        [Fact]
        public async Task DeleteVoteAsync_ReturnsNotFound_WhenVoteDoesNotExist()
        {
            // Arrange
            var user = new BritterUser { Id = Guid.NewGuid() };
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _mockRepo.Setup(repo => repo.GetVoteAsync(It.IsAny<Guid>(), user.Id)).ReturnsAsync((Vote?)null);

            // Act
            var result = await _controller.DeleteVoteAsync(Guid.NewGuid());

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            _mockRepo.Verify(repo => repo.DeleteVoteAsync(It.IsAny<Vote>()), Times.Never);
        }
    }
}



