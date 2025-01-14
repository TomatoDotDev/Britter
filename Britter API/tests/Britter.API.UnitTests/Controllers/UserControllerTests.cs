using Britter.API.Controllers;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Britter.DTO.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Britter.API.UnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IBritterUserRepo> _mockRepo;
        private readonly Mock<UserManager<BritterUser>> _mockUserManager;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockRepo = new Mock<IBritterUserRepo>();
            var store = new Mock<IUserStore<BritterUser>>();
            _mockUserManager = new Mock<UserManager<BritterUser>>(store.Object, null, null, null, null, null, null, null, null);
            _controller = new UserController(_mockRepo.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task GetUsers_ReturnsUsers()
        {
            // Arrange
            var users = new List<BritterUser>
            {
                new BritterUser { Id = Guid.NewGuid(), UserName = "testuser", Email = "test@example.com" }
            };
            _mockUserManager.Setup(m => m.Users).Returns(users.AsQueryable());
            _mockRepo.Setup(repo => repo.GetUserAsync(It.IsAny<IQueryable<BritterUser>>(), It.IsAny<BritterUserSearchQueryDTO>())).ReturnsAsync(users);

            // Act
            var result = await _controller.GetUsers(new BritterUserSearchQueryDTO());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<BritterUserResponseDTO>>(okResult.Value);
            Assert.Single(returnValue);
            Assert.Equal("testuser", returnValue.First().UserName);
        }

        [Fact]
        public async Task ChangeUserLockStatus_ReturnsOk_WhenUserIsLocked()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new BritterUser { Id = Guid.Parse(userId) };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.ChangeUserLockStatus(userId, true);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockUserManager.Verify(m => m.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue), Times.Once);
        }

        [Fact]
        public async Task ChangeUserLockStatus_ReturnsOk_WhenUserIsUnlocked()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new BritterUser { Id = Guid.Parse(userId) };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.SetLockoutEndDateAsync(user, null)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.ChangeUserLockStatus(userId, false);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockUserManager.Verify(m => m.SetLockoutEndDateAsync(user, null), Times.Once);
        }

        [Fact]
        public async Task ChangeUserLockStatus_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((BritterUser?)null);

            // Act
            var result = await _controller.ChangeUserLockStatus(Guid.NewGuid().ToString(), true);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            _mockUserManager.Verify(m => m.SetLockoutEndDateAsync(It.IsAny<BritterUser>(), It.IsAny<DateTimeOffset?>()), Times.Never);
        }

        [Fact]
        public async Task ChangeUserLockStatus_ReturnsConflict_WhenLockStatusChangeFails()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new BritterUser { Id = Guid.Parse(userId) };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue)).ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _controller.ChangeUserLockStatus(userId, true);

            // Assert
            var conflictResult = Assert.IsType<ConflictResult>(result);
            _mockUserManager.Verify(m => m.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_ReturnsOk_WhenUserIsDeleted()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new BritterUser { Id = userId };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockUserManager.Verify(m => m.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((BritterUser?)null);

            // Act
            var result = await _controller.DeleteUser(Guid.NewGuid());

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            _mockUserManager.Verify(m => m.DeleteAsync(It.IsAny<BritterUser>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUser_ReturnsConflict_WhenUserDeletionFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new BritterUser { Id = userId };
            _mockUserManager.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            var conflictResult = Assert.IsType<ConflictResult>(result);
            _mockUserManager.Verify(m => m.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task IsUserAdmin_ReturnsTrue_WhenUserIsAdmin()
        {
            // Arrange
            var user = new BritterUser { Id = Guid.NewGuid(), UserName = "adminUser" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.IsInRoleAsync(user, "admin")).ReturnsAsync(true);

            // Act
            var result = await _controller.IsUserAdmin();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<bool>(okResult.Value);
            Assert.True(returnValue);
        }

        [Fact]
        public async Task IsUserAdmin_ReturnsFalse_WhenUserIsNotAdmin()
        {
            // Arrange
            var user = new BritterUser { Id = Guid.NewGuid(), UserName = "regularUser" };
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _mockUserManager.Setup(m => m.IsInRoleAsync(user, "admin")).ReturnsAsync(false);

            // Act
            var result = await _controller.IsUserAdmin();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<bool>(okResult.Value);
            Assert.False(returnValue);
        }

        [Fact]
        public async Task IsUserAdmin_ReturnsNotFound_WhenUserIsNotFound()
        {
            // Arrange
            _mockUserManager.Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((BritterUser?)null);

            // Act
            var result = await _controller.IsUserAdmin();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}


