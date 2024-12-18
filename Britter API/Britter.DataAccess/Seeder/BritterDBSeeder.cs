using Britter.Models;
using Britter.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Britter.DataAccess.Seeder
{
    public class BritterDBSeeder : IBritterDBSeeder
    {
        private readonly IPasswordHasher<BritterUser> _hasher;

        public BritterDBSeeder(IPasswordHasher<BritterUser> hasher)
        {
            _hasher = hasher;
        }

        public IEnumerable<BritterUser> SeedUsers()
        {
            return
            [
                new()
                {
                    UserName = "Administrator",
                    Email = "administrator@britter.com",
                    PasswordHash = _hasher.HashPassword(null, "Password"),
                    AcceptedCodeOfConduct = true,
                    CreatedAt = DateTime.Now,
                    LockoutEnabled = false,
                },
                new()
                {
                    UserName = "TestUser",
                    Email = "testuser@britter.com",
                    PasswordHash = _hasher.HashPassword(null, "Password"),
                    AcceptedCodeOfConduct = true,
                    CreatedAt = DateTime.Now,
                    LockoutEnabled = false,
                },
            ];
        }
    }
}
