using Britter.API.Controllers;
using Britter.API.Services;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Britter.DTO.Response;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Britter.API.UnitTests.Controllers
{
    public class TopicControllerTests
    {
        private readonly Mock<ITopicRepo> _mockRepo;
        private readonly Mock<IUserUtilityService> _mockUserUtility;
        private readonly TopicController _controller;

        public TopicControllerTests()
        {
            _mockRepo = new Mock<ITopicRepo>();
            _mockUserUtility = new Mock<IUserUtilityService>();
            _controller = new TopicController(_mockRepo.Object, _mockUserUtility.Object);
        }

        [Fact]
        public async Task GetTopics_ReturnsTopics()
        {
            // Arrange
            var topics = new List<Topic>
            {
                new Topic { TopicId = Guid.NewGuid(), Title = "Test title" }
            };
            _mockRepo.Setup(repo => repo.GetTopicAsync(It.IsAny<TopicQueryDTO>())).ReturnsAsync(topics);

            // Act
            var result = await _controller.GetTopics(new TopicQueryDTO());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TopicResponseDTO>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal("Test title", returnValue.First().Title);
        }

        [Fact]
        public async Task CreateTopic_ReturnsCreated_WhenUserIsAuthenticated()
        {
            // Arrange
            var user = new BritterUser { Id = Guid.NewGuid() };
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var topicCreateDto = new TopicCreateDTO { Title = "Test title", Description = "Test description" };

            // Act
            var result = await _controller.CreateTopic(topicCreateDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            _mockRepo.Verify(repo => repo.CreateTopicAsync(It.IsAny<Topic>()), Times.Once);
        }

        [Fact]
        public async Task CreateTopic_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((BritterUser?)null);

            var topicCreateDto = new TopicCreateDTO { Title = "Test title", Description = "Test description" };

            // Act
            var result = await _controller.CreateTopic(topicCreateDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
            _mockRepo.Verify(repo => repo.CreateTopicAsync(It.IsAny<Topic>()), Times.Never);
        }

        [Fact]
        public async Task UpdateTopic_ReturnsOk_WhenTopicIsUpdated()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            var topic = new Topic { TopicId = topicId, Title = "Old title", AuthorId = Guid.NewGuid() };
            _mockRepo.Setup(repo => repo.GetTopicAsync(It.IsAny<TopicQueryDTO>())).ReturnsAsync(new List<Topic> { topic });
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new BritterUser { Id = topic.AuthorId });
            _mockUserUtility.Setup(u => u.VerifyAdminRole(It.IsAny<BritterUser>())).ReturnsAsync(false);

            var topicCreateDto = new TopicCreateDTO { Title = "Updated title", Description = "Updated description" };

            // Act
            var result = await _controller.UpdateTopic(topicId, topicCreateDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockRepo.Verify(repo => repo.UpdateTopicAsync(It.IsAny<Topic>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTopic_ReturnsNotFound_WhenTopicDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetTopicAsync(It.IsAny<TopicQueryDTO>())).ReturnsAsync(new List<Topic>());

            var topicCreateDto = new TopicCreateDTO { Title = "Updated title", Description = "Updated description" };

            // Act
            var result = await _controller.UpdateTopic(Guid.NewGuid(), topicCreateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            _mockRepo.Verify(repo => repo.UpdateTopicAsync(It.IsAny<Topic>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTopic_ReturnsOk_WhenTopicIsDeleted()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            var topic = new Topic { TopicId = topicId, Title = "Test title", AuthorId = Guid.NewGuid() };
            _mockRepo.Setup(repo => repo.GetTopicAsync(It.IsAny<TopicQueryDTO>())).ReturnsAsync(new List<Topic> { topic });
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new BritterUser { Id = topic.AuthorId });
            _mockUserUtility.Setup(u => u.VerifyAdminRole(It.IsAny<BritterUser>())).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteTopic(topicId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockRepo.Verify(repo => repo.DeleteTopicAsync(It.IsAny<Topic>()), Times.Once);
        }

        [Fact]
        public async Task DeleteTopic_ReturnsNotFound_WhenTopicDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetTopicAsync(It.IsAny<TopicQueryDTO>())).ReturnsAsync(new List<Topic>());

            // Act
            var result = await _controller.DeleteTopic(Guid.NewGuid());

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            _mockRepo.Verify(repo => repo.DeleteTopicAsync(It.IsAny<Topic>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTopic_ReturnsUnauthorized_WhenUserIsNotAuthorized()
        {
            // Arrange
            var topicId = Guid.NewGuid();
            var topic = new Topic { TopicId = topicId, Title = "Test title", AuthorId = Guid.NewGuid() };
            _mockRepo.Setup(repo => repo.GetTopicAsync(It.IsAny<TopicQueryDTO>())).ReturnsAsync(new List<Topic> { topic });
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new BritterUser { Id = Guid.NewGuid() });
            _mockUserUtility.Setup(u => u.VerifyAdminRole(It.IsAny<BritterUser>())).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteTopic(topicId);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
            _mockRepo.Verify(repo => repo.DeleteTopicAsync(It.IsAny<Topic>()), Times.Never);
        }
    }
}


