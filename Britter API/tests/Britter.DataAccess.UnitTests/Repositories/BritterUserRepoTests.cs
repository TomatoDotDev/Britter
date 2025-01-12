using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Britter.DataAccess.Tests.Repositories
{
    public class BritterUserRepoTests
    {
        private readonly Mock<DbSet<BritterUser>> _mockSet;
        private readonly Mock<DbContext> _mockContext;
        private readonly BritterUserRepo _repo;

        public BritterUserRepoTests()
        {
            _mockSet = new Mock<DbSet<BritterUser>>();
            _mockContext = new Mock<DbContext>();
            _mockContext.Setup(m => m.Set<BritterUser>()).Returns(_mockSet.Object);
            _repo = new BritterUserRepo();
        }

        [Fact]
        public async Task GetUserAsync_FiltersByEmail()
        {
            // Arrange
            var users = new List<BritterUser>
            {
                new BritterUser { Email = "test@example.com" },
                new BritterUser { Email = "other@example.com" }
            }.AsQueryable();

            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var query = new BritterUserSearchQueryDTO { Email = "test@example.com", PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _repo.GetUserAsync(users, query);

            // Assert
            Assert.Single(result);
            Assert.Equal("test@example.com", result.First().Email);
        }

        [Fact]
        public async Task GetUserAsync_FiltersByUserName()
        {
            // Arrange
            var users = new List<BritterUser>
            {
                new BritterUser { UserName = "testuser" },
                new BritterUser { UserName = "otheruser" }
            }.AsQueryable();

            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var query = new BritterUserSearchQueryDTO { UserName = "testuser", PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _repo.GetUserAsync(users, query);

            // Assert
            Assert.Single(result);
            Assert.Equal("testuser", result.First().UserName);
        }

        [Fact]
        public async Task GetUserAsync_FiltersByIsBlocked()
        {
            // Arrange
            var users = new List<BritterUser>
            {
                new BritterUser { LockoutEnd = DateTime.Now.AddDays(1) },
                new BritterUser { LockoutEnd = null }
            }.AsQueryable();

            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var query = new BritterUserSearchQueryDTO { IsBlocked = true, PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _repo.GetUserAsync(users, query);

            // Assert
            Assert.Single(result);
            Assert.NotNull(result.First().LockoutEnd);
        }

        [Fact]
        public async Task GetUserAsync_FiltersById()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var users = new List<BritterUser>
            {
                new BritterUser { Id = userId },
                new BritterUser { Id = Guid.NewGuid() }
            }.AsQueryable();

            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var query = new BritterUserSearchQueryDTO { Id = userId, PageNumber = 1, PageSize = 10 };

            // Act
            var result = await _repo.GetUserAsync(users, query);

            // Assert
            Assert.Single(result);
            Assert.Equal(userId, result.First().Id);
        }

        [Fact]
        public async Task GetUserAsync_PaginatesResults()
        {
            // Arrange
            var users = new List<BritterUser>
            {
                new BritterUser { UserName = "user1" },
                new BritterUser { UserName = "user2" },
                new BritterUser { UserName = "user3" }
            }.AsQueryable();

            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.Provider).Returns(users.Provider);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.Expression).Returns(users.Expression);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
            _mockSet.As<IQueryable<BritterUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            var query = new BritterUserSearchQueryDTO { PageNumber = 2, PageSize = 1 };

            // Act
            var result = await _repo.GetUserAsync(users, query);

            // Assert
            Assert.Single(result);
            Assert.Equal("user2", result.First().UserName);
        }
    }
}
