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
    public class PostControllerTests
    {
        private readonly Mock<IPostRepo> _mockRepo;
        private readonly Mock<IUserUtilityService> _mockUserUtility;
        private readonly PostController _controller;

        public PostControllerTests()
        {
            _mockRepo = new Mock<IPostRepo>();
            _mockUserUtility = new Mock<IUserUtilityService>();
            _controller = new PostController(_mockRepo.Object, _mockUserUtility.Object);
        }

        [Fact]
        public async Task GetPostAsync_ReturnsPosts()
        {
            // Arrange
            var posts = new List<Post>
            {
                new Post { PostId = Guid.NewGuid(), Content = "Test content" }
            };
            _mockRepo.Setup(repo => repo.GetPostAsync(It.IsAny<PostQueryDTO>())).ReturnsAsync(posts);

            // Act
            var result = await _controller.GetPostAsync(new PostQueryDTO());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PostResponseDTO>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal("Test content", returnValue.First().Content);
        }

        [Fact]
        public async Task CreatePostAsync_ReturnsCreated_WhenUserIsAuthenticated()
        {
            // Arrange
            var user = new BritterUser { Id = Guid.NewGuid() };
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var postCreateDto = new PostCreateDTO { Content = "Test content", TopicId = Guid.NewGuid() };

            // Act
            var result = await _controller.CreatePostAsync(postCreateDto);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            _mockRepo.Verify(repo => repo.CreatePostAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task CreatePostAsync_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((BritterUser?)null);

            var postCreateDto = new PostCreateDTO { Content = "Test content", TopicId = Guid.NewGuid() };

            // Act
            var result = await _controller.CreatePostAsync(postCreateDto);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
            _mockRepo.Verify(repo => repo.CreatePostAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePostAsync_ReturnsOk_WhenPostIsUpdated()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var post = new Post { PostId = postId, Content = "Old content", AuthorId = Guid.NewGuid() };
            _mockRepo.Setup(repo => repo.GetPostAsync(It.IsAny<PostQueryDTO>())).ReturnsAsync(new List<Post> { post });
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new BritterUser { Id = post.AuthorId });
            _mockUserUtility.Setup(u => u.VerifyAdminRole(It.IsAny<BritterUser>())).ReturnsAsync(false);

            var postCreateDto = new PostCreateDTO { Content = "Updated content", TopicId = post.TopicId };

            // Act
            var result = await _controller.UpdatePostAsync(postId, postCreateDto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockRepo.Verify(repo => repo.UpdatePostAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePostAsync_ReturnsNotFound_WhenPostDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetPostAsync(It.IsAny<PostQueryDTO>())).ReturnsAsync(new List<Post>());

            var postCreateDto = new PostCreateDTO { Content = "Updated content", TopicId = Guid.NewGuid() };

            // Act
            var result = await _controller.UpdatePostAsync(Guid.NewGuid(), postCreateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            _mockRepo.Verify(repo => repo.UpdatePostAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task DeletePostAsync_ReturnsOk_WhenPostIsDeleted()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var post = new Post { PostId = postId, Content = "Test content", AuthorId = Guid.NewGuid() };
            _mockRepo.Setup(repo => repo.GetPostAsync(It.IsAny<PostQueryDTO>())).ReturnsAsync(new List<Post> { post });
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new BritterUser { Id = post.AuthorId });
            _mockUserUtility.Setup(u => u.VerifyAdminRole(It.IsAny<BritterUser>())).ReturnsAsync(false);

            // Act
            var result = await _controller.DeletePostAsync(postId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockRepo.Verify(repo => repo.UpdatePostAsync(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task DeletePostAsync_ReturnsNotFound_WhenPostDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetPostAsync(It.IsAny<PostQueryDTO>())).ReturnsAsync(new List<Post>());

            // Act
            var result = await _controller.DeletePostAsync(Guid.NewGuid());

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            _mockRepo.Verify(repo => repo.UpdatePostAsync(It.IsAny<Post>()), Times.Never);
        }

        [Fact]
        public async Task DeletePostAsync_ReturnsUnauthorized_WhenUserIsNotAuthorized()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var post = new Post { PostId = postId, Content = "Test content", AuthorId = Guid.NewGuid() };
            _mockRepo.Setup(repo => repo.GetPostAsync(It.IsAny<PostQueryDTO>())).ReturnsAsync(new List<Post> { post });
            _mockUserUtility.Setup(u => u.GetUserFromClaimAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new BritterUser { Id = Guid.NewGuid() });
            _mockUserUtility.Setup(u => u.VerifyAdminRole(It.IsAny<BritterUser>())).ReturnsAsync(false);

            // Act
            var result = await _controller.DeletePostAsync(postId);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedResult>(result);
            _mockRepo.Verify(repo => repo.UpdatePostAsync(It.IsAny<Post>()), Times.Never);
        }
    }
}

