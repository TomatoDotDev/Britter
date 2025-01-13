using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Microsoft.EntityFrameworkCore;

namespace Britter.DataAccess.UnitTests.Repositories
{
    public class PostRepoTests
    {

        [Fact]
        public async Task GetPostAsync_FiltersById()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new PostRepo(ctx);
            var postId = Guid.NewGuid();
            var posts = new List<Post>
            {
                new Post { PostId = postId },
                new Post { PostId = Guid.NewGuid() }
            };

            await ctx.Posts.AddRangeAsync(posts);
            await ctx.SaveChangesAsync();

            var query = new PostQueryDTO { Id = postId };

            // Act
            var result = await repo.GetPostAsync(query);

            // Assert
            Assert.Single(result);
            Assert.Equal(postId, result.First().PostId);
        }

        [Fact]
        public async Task GetPostAsync_FiltersByTopicId()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new PostRepo(ctx);
            var topicId = Guid.NewGuid();
            var posts = new List<Post>
            {
                new Post { TopicId = topicId },
                new Post { TopicId = Guid.NewGuid() }
            };

            await ctx.Posts.AddRangeAsync(posts);
            await ctx.SaveChangesAsync();

            var query = new PostQueryDTO { TopicId = topicId };

            // Act
            var result = await repo.GetPostAsync(query);

            // Assert
            Assert.Single(result);
            Assert.Equal(topicId, result.First().TopicId);
        }

        [Fact]
        public async Task GetPostAsync_FiltersByAuthor()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new PostRepo(ctx);
            var author = "testuser";
            var posts = new List<Post>
            {
                new Post { User = new BritterUser { UserName = author } },
                new Post { User = new BritterUser { UserName = "otheruser" } }
            };

            await ctx.Posts.AddRangeAsync(posts);
            await ctx.SaveChangesAsync();

            var query = new PostQueryDTO { Author = author };

            // Act
            var result = await repo.GetPostAsync(query);

            // Assert
            Assert.Single(result);
            Assert.Equal(author, result.First().User.UserName);
        }

        [Fact]
        public async Task GetPostAsync_FiltersByContent()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new PostRepo(ctx);
            var content = "test content";
            var posts = new List<Post>
            {
                new Post { Content = content },
                new Post { Content = "other content" }
            };

            await ctx.Posts.AddRangeAsync(posts);
            await ctx.SaveChangesAsync();

            var query = new PostQueryDTO { Content = content };

            // Act
            var result = await repo.GetPostAsync(query);

            // Assert
            Assert.Single(result);
            Assert.Contains(content, result.First().Content);
        }

        [Fact]
        public async Task GetPostAsync_FiltersByShowDeletedPosts()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new PostRepo(ctx);
            var posts = new List<Post>
            {
                new Post { IsDeleted = true },
                new Post { IsDeleted = false }
            };

            await ctx.Posts.AddRangeAsync(posts);
            await ctx.SaveChangesAsync();

            var query = new PostQueryDTO { ShowDeletedPosts = false };

            // Act
            var result = await repo.GetPostAsync(query);

            // Assert
            Assert.Single(result);
            Assert.False(result.First().IsDeleted);
        }

        [Fact]
        public async Task CreatePostAsync_AddsPost()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new PostRepo(ctx);
            var post = new Post { PostId = Guid.NewGuid(),};

            // Act
            await repo.CreatePostAsync(post);

            // Assert
            var addedPost = await ctx.Posts.FindAsync(post.PostId);
            Assert.NotNull(addedPost);
        }

        [Fact]
        public async Task UpdatePostAsync_UpdatesPost()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new PostRepo(ctx);
            var post = new Post { PostId = Guid.NewGuid() };
            await ctx.Posts.AddAsync(post);
            await ctx.SaveChangesAsync();

            post.Content = "Updated content";

            // Act
            await repo.UpdatePostAsync(post);

            // Assert
            var updatedPost = await ctx.Posts.FindAsync(post.PostId);
            Assert.Equal("Updated content", updatedPost.Content);
        }

        [Fact]
        public async Task DeletePostAsync_DeletesPost()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new PostRepo(ctx);
            var post = new Post { PostId = Guid.NewGuid(),
            Content = "test"};
            await ctx.Posts.AddAsync(post);
            await ctx.SaveChangesAsync();

            // Act
            await repo.DeletePostAsync(post);

            // Assert
            var deletedPost = await ctx.Posts.FindAsync(post.PostId);
            Assert.Null(deletedPost);
        }
    }
}

