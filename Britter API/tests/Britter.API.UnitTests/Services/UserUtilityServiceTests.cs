using Britter.API.Services;
using Britter.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Britter.API.UnitTests.Services
{
    public class UserUtilityServiceTests
    {
        private readonly Mock<UserManager<BritterUser>> _mockUserManager;
        private readonly UserUtilityService _service;

        public UserUtilityServiceTests()
        {
            var store = new Mock<IUserStore<BritterUser>>();
            _mockUserManager = new Mock<UserManager<BritterUser>>(store.Object, null, null, null, null, null, null, null, null);
            _service = new UserUtilityService(_mockUserManager.Object);
        }

        [Fact]
        public async Task GetUserFromClaimAsync_ReturnsUser_WhenUserIdIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new BritterUser { Id = Guid.Parse(userId) };
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            _mockUserManager.Setup(m => m.FindByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _service.GetUserFromClaimAsync(claimsPrincipal);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id.ToString());
        }

        [Fact]
        public async Task GetUserFromClaimAsync_ReturnsNull_WhenUserIdIsInvalid()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "invalid-user-id")
            }));

            _mockUserManager.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((BritterUser?)null);

            // Act
            var result = await _service.GetUserFromClaimAsync(claimsPrincipal);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserFromClaimAsync_ReturnsNull_WhenUserIdIsNull()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            var result = await _service.GetUserFromClaimAsync(claimsPrincipal);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task VerifyAdminRole_ReturnsTrue_WhenUserIsAdmin()
        {
            // Arrange
            var user = new BritterUser { UserName = "adminUser" };
            _mockUserManager.Setup(m => m.IsInRoleAsync(user, "admin")).ReturnsAsync(true);

            // Act
            var result = await _service.VerifyAdminRole(user);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task VerifyAdminRole_ReturnsFalse_WhenUserIsNotAdmin()
        {
            // Arrange
            var user = new BritterUser { UserName = "regularUser" };
            _mockUserManager.Setup(m => m.IsInRoleAsync(user, "admin")).ReturnsAsync(false);

            // Act
            var result = await _service.VerifyAdminRole(user);

            // Assert
            Assert.False(result);
        }
    }
}

