using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Britter.DataAccess.UnitTests.Repositories
{
    public class BritterUserRepoTests
    {
        [Fact]
        public async Task GetUserAsync_FiltersByEmail()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new BritterUserRepo();
            var users = new List<BritterUser>
            {
                new BritterUser { Email = "test@example.com" },
                new BritterUser { Email = "other@example.com" }
            };

            await ctx.Set<BritterUser>().AddRangeAsync(users);
            await ctx.SaveChangesAsync();

            var query = new BritterUserSearchQueryDTO { Email = "test@example.com", PageNumber = 1, PageSize = 10 };

            // Act
            var result = await repo.GetUserAsync(ctx.Set<BritterUser>().AsQueryable(), query);

            // Assert
            Assert.Single(result);
            Assert.Equal("test@example.com", result.First().Email);
        }

        [Fact]
        public async Task GetUserAsync_FiltersByUserName()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new BritterUserRepo();
            var username = "sometestuser";
            var users = new List<BritterUser>
            {
                new BritterUser { UserName = username },
                new BritterUser { UserName = "otheruser" }
            };

            await ctx.Set<BritterUser>().AddRangeAsync(users);
            await ctx.SaveChangesAsync();

            var query = new BritterUserSearchQueryDTO { UserName = username, PageNumber = 1, PageSize = 10 };

            // Act
            var result = await repo.GetUserAsync(ctx.Set<BritterUser>().AsQueryable(), query);

            // Assert
            Assert.Single(result);
            Assert.Equal(username, result.First().UserName);
        }

        [Fact]
        public async Task GetUserAsync_FiltersByIsBlocked()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new BritterUserRepo();
            var users = new List<BritterUser>
            {
                new BritterUser { LockoutEnd = DateTime.Now.AddDays(1) },
                new BritterUser { LockoutEnd = null }
            };

            await ctx.Set<BritterUser>().AddRangeAsync(users);
            await ctx.SaveChangesAsync();

            var query = new BritterUserSearchQueryDTO { IsBlocked = true, PageNumber = 1, PageSize = 10 };

            // Act
            var result = await repo.GetUserAsync(ctx.Set<BritterUser>().AsQueryable(), query);

            // Assert
            Assert.Single(result);
            Assert.NotNull(result.First().LockoutEnd);
        }

        [Fact]
        public async Task GetUserAsync_FiltersById()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new BritterUserRepo();
            var userId = Guid.NewGuid();
            var users = new List<BritterUser>
            {
                new BritterUser { Id = userId },
                new BritterUser { Id = Guid.NewGuid() }
            };

            await ctx.Set<BritterUser>().AddRangeAsync(users);
            await ctx.SaveChangesAsync();

            var query = new BritterUserSearchQueryDTO { Id = userId, PageNumber = 1, PageSize = 10 };

            // Act
            var result = await repo.GetUserAsync(ctx.Set<BritterUser>().AsQueryable(), query);

            // Assert
            Assert.Single(result);
            Assert.Equal(userId, result.First().Id);
        }

        [Fact]
        public async Task GetUserAsync_PaginatesResults()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new BritterUserRepo();
            var users = new List<BritterUser>
            {
                new BritterUser { UserName = "user1" },
                new BritterUser { UserName = "user2" },
                new BritterUser { UserName = "user3" }
            };

            await ctx.Set<BritterUser>().AddRangeAsync(users);
            await ctx.SaveChangesAsync();

            var query = new BritterUserSearchQueryDTO { PageNumber = 2, PageSize = 1 };

            // Act
            var result = await repo.GetUserAsync(ctx.Set<BritterUser>().AsQueryable(), query);

            // Assert
            Assert.Single(result);
            Assert.Equal("user2", result.First().UserName);
        }
    }
}

