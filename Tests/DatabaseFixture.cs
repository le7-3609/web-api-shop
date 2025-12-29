using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Tests
{
    public class DatabaseFixture : IDisposable
    {
        public MyShopContext Context { get; private set; }

        public DatabaseFixture()
        {

            // Set up the test database connection and initialize the context
            var options = new DbContextOptionsBuilder<MyShopContext>()
                .UseSqlServer("Data Source=ELISHEVA;Initial Catalog=Test;Integrated Security=True;Encrypt=True;Trust Server Certificate=True")
                .Options;
            Context = new MyShopContext(options);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            // Clean up the test database after all tests are completed
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
