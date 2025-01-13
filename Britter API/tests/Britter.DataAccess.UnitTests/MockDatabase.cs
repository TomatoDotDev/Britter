using Britter.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Britter.DataAccess.UnitTests
{
    public static class MockDatabase
    {
        public static BritterDBContext GenerateDb()
        {
            var options = new DbContextOptionsBuilder<BritterDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new BritterDBContext(options);
        }
    }
}
