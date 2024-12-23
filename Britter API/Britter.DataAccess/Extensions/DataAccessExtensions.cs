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
            //services.AddSingleton(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
            services.AddDbContext<BritterDBContext>(options => 
            {
                // TODO: change me to new provider.
                options.UseSqlite("Data Source=database.db");
            });

            services.AddScoped<IBritterUserRepo, BritterUserRepo>();

            return services;
        }
    }
}
