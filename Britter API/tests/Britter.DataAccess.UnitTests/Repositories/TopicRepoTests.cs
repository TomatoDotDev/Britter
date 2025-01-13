using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DTO.Request;
using Microsoft.EntityFrameworkCore;

namespace Britter.DataAccess.UnitTests.Repositories
{
    public class TopicRepoTests
    {
        [Fact]
        public async Task CreateTopicAsync_AddsTopic()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new TopicRepo(ctx);
            var topic = new Topic { TopicId = Guid.NewGuid() };

            // Act
            await repo.CreateTopicAsync(topic);

            // Assert
            var addedTopic = await ctx.Topics.FindAsync(topic.TopicId);
            Assert.NotNull(addedTopic);
        }

        [Fact]
        public async Task UpdateTopicAsync_UpdatesTopic()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new TopicRepo(ctx);
            var topic = new Topic { TopicId = Guid.NewGuid() };
            await ctx.Topics.AddAsync(topic);
            await ctx.SaveChangesAsync();

            topic.Title = "Updated title";

            // Act
            await repo.UpdateTopicAsync(topic);

            // Assert
            var updatedTopic = await ctx.Topics.FindAsync(topic.TopicId);
            Assert.Equal("Updated title", updatedTopic.Title);
        }

        [Fact]
        public async Task DeleteTopicAsync_DeletesTopic()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new TopicRepo(ctx);
            var topic = new Topic { TopicId = Guid.NewGuid() };
            await ctx.Topics.AddAsync(topic);
            await ctx.SaveChangesAsync();

            // Act
            await repo.DeleteTopicAsync(topic);

            // Assert
            var deletedTopic = await ctx.Topics.FindAsync(topic.TopicId);
            Assert.Null(deletedTopic);
        }

        [Fact]
        public async Task GetTopicAsync_FiltersById()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new TopicRepo(ctx);
            var topicId = Guid.NewGuid();
            var topics = new List<Topic>
            {
                new Topic { TopicId = topicId },
                new Topic { TopicId = Guid.NewGuid() }
            };

            await ctx.Topics.AddRangeAsync(topics);
            await ctx.SaveChangesAsync();

            var query = new TopicQueryDTO { Id = topicId };

            // Act
            var result = await repo.GetTopicAsync(query);

            // Assert
            Assert.Single(result);
            Assert.Equal(topicId, result.First().TopicId);
        }

        [Fact]
        public async Task GetTopicAsync_FiltersByTitle()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new TopicRepo(ctx);
            var title = "test title";
            var topics = new List<Topic>
            {
                new Topic { Title = title },
                new Topic { Title = "other title" }
            };

            await ctx.Topics.AddRangeAsync(topics);
            await ctx.SaveChangesAsync();

            var query = new TopicQueryDTO { Title = title };

            // Act
            var result = await repo.GetTopicAsync(query);

            // Assert
            Assert.Single(result);
            Assert.Contains(title, result.First().Title, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task GetTopicAsync_FiltersByDescription()
        {
            // Arrange
            var ctx = MockDatabase.GenerateDb();
            var repo = new TopicRepo(ctx);
            var description = "test description";
            var topics = new List<Topic>
            {
                new Topic { Description = description },
                new Topic { Description = "other description" }
            };

            await ctx.Topics.AddRangeAsync(topics);
            await ctx.SaveChangesAsync();

            var query = new TopicQueryDTO { Description = description };

            // Act
            var result = await repo.GetTopicAsync(query);

            // Assert
            Assert.Single(result);
            Assert.Contains(description, result.First().Description, StringComparison.OrdinalIgnoreCase);
        }
    }
}

