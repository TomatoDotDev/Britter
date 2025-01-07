using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using Britter.DTO.Request;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.DataAccess.Repositories
{
    /// <inheritdoc cref="IPostRepo"/>
    public class PostRepo : IPostRepo
    {
        private readonly BritterDBContext _context;

        public PostRepo(BritterDBContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Post>> GetPostAsync(PostQueryDTO query)
        {
            var posts = _context.Posts.AsQueryable();
            if (query.Id.HasValue)
            {
                posts = posts.Where(p => p.PostId == query.Id);
            }
            if (query.TopicId.HasValue)
            {
                posts = posts.Where(p => p.TopicId == query.TopicId);
            }
            if (!string.IsNullOrEmpty(query.Author))
            {
                posts = posts.Where(p => p.User.UserName == query.Author);
            }
            if (!string.IsNullOrEmpty(query.Content))
            {
                posts = posts.Where(p => p.Content.Contains(query.Content));
            }

            if (!query.ShowDeletedPosts)
            {
                posts = posts.Where(p => !p.IsDeleted);
            }

            // we just want top level posts here as all are nested.
            posts = posts.Where(p => p.ParentPostId == null);
            return posts;
        }

        /// <inheritdoc />
        public async Task CreatePostAsync(Post Post)
        {
            await _context.Posts.AddAsync(Post);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdatePostAsync(Post Post)
        {
            _context.Posts.Update(Post);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeletePostAsync(Post Post)
        {
            _context.Posts.Remove(Post);
            await _context.SaveChangesAsync();
        }
    }
}
