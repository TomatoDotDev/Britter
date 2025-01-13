using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Britter.DataAccess.UnitTests.Repositories
{
    public class VoteRepoTests
    {

        [Fact]
        public async Task CreateVoteAsync_AddsVote()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new VoteRepo(ctx);
            var vote = new Vote { PostId = Guid.NewGuid(), UserId = Guid.NewGuid() };

            // Act
            await repo.CreateVoteAsync(vote);

            // Assert
            var addedVote = await ctx.Votes.FindAsync(vote.PostId, vote.UserId);
            Assert.NotNull(addedVote);
        }

        [Fact]
        public async Task DeleteVoteAsync_DeletesVote()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new VoteRepo(ctx);
            var vote = new Vote { PostId = Guid.NewGuid(), UserId = Guid.NewGuid() };
            await ctx.Votes.AddAsync(vote);
            await ctx.SaveChangesAsync();

            // Act
            await repo.DeleteVoteAsync(vote);

            // Assert
            var deletedVote = await ctx.Votes.FindAsync(vote.PostId, vote.UserId);
            Assert.Null(deletedVote);
        }

        [Fact]
        public async Task GetVoteAsync_ReturnsVote()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new VoteRepo(ctx);
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var vote = new Vote { PostId = postId, UserId = userId };

            await ctx.Votes.AddAsync(vote);
            await ctx.SaveChangesAsync();

            // Act
            var result = await repo.GetVoteAsync(postId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(postId, result.PostId);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task GetVoteAsync_ReturnsNullWhenVoteNotFound()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new VoteRepo(ctx);
            var postId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            // Act
            var result = await repo.GetVoteAsync(postId, userId);

            // Assert
            Assert.Null(result);
        }
    }
}

