using Britter.DataAccess.Models;

namespace Britter.DataAccess.Seeder
{
    /// <summary>
    /// Database seeder for the Britter database.
    /// </summary>
    public interface IBritterDBSeeder
    {
        /// <summary>
        /// Seed the database with users.
        /// </summary>
        /// <returns>A list of users.</returns>
        IEnumerable<BritterUser> SeedUsers();
    }
}