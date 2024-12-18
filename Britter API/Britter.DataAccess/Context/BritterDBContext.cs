using Britter.DataAccess.Seeder;
using Britter.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Britter.DataAccess.Context
{
    /// <summary>
    /// Britter Database Context Session service derived from <see cref="DbContext"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class BritterDBContext : IdentityDbContext<BritterUser, IdentityRole<Guid>, Guid>
    {

        /// <summary>
        /// Initialises an instance of <see cref="BritterDBContext"/>.
        /// </summary>
        /// <param name="options">The DB context options.</param>
        /// <param name="seeder">The DB seeder service.</param>
        public BritterDBContext(DbContextOptions<BritterDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Database set for all report records.
        /// </summary>
        public DbSet<Report> Reports { get; set; }

        /// <summary>
        /// Database set for all topic records.
        /// </summary>
        public DbSet<Topic> Topics { get; set; }

        /// <summary>
        /// Database set for all post records.
        /// </summary>
        public DbSet<Post> Posts { get; set; }

        /// <summary>
        /// Database set for all vote records.
        /// </summary>
        public DbSet<Vote> Votes { get; set; }
    }
}
