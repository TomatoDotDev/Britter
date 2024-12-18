using Britter.DataAccess.Models;
using Britter.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace Britter.DataAccess.Seeder
{
    /// <inheritdoc cref="IBritterDBSeeder"/>/>
    public class BritterDBSeeder : IBritterDBSeeder
    {
        private readonly IPasswordHasher<BritterUser> _hasher;

        /// <summary>
        /// Constructor for <see cref="BritterDBSeeder"/>.
        /// </summary>
        /// <param name="hasher">Password hasher service.</param>
        public BritterDBSeeder(IPasswordHasher<BritterUser> hasher)
        {
            _hasher = hasher;
        }

        /// <inheritdoc />
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
