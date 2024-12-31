using Britter.DataAccess.Context;
using Britter.DataAccess.Models;
using Britter.DataAccess.Repositories;
using Britter.DataAccess.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Britter.DataAccess.Extensions
{
    public static class DataAccessExtensions
    {
        /// <summary>
        /// Registers the database access services to the service container.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <returns>An updated service collection.</returns>
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
        {
            services.AddDbContext<BritterDBContext>(options => 
            {
                options.UseLazyLoadingProxies();
                options.UseSqlite("Data Source=database.db");
            });

            services.AddScoped<IBritterUserRepo, BritterUserRepo>();
            services.AddScoped<ITopicRepo, TopicRepo>();
            services.AddScoped<IPostRepo, PostRepo>();

            return services;
        }
    }
}
