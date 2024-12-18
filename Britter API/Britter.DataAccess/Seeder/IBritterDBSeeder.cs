using Britter.Models;

namespace Britter.DataAccess.Seeder
{
    public interface IBritterDBSeeder
    {
        IEnumerable<BritterUser> SeedUsers();
    }
}