using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Tests.IntegretionTests
{
    public class DatabaseFixture : IDisposable
    {
        public MyShopContext Context { get; private set; }

        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<MyShopContext>()
                .UseSqlServer("Data Source=ELISHEVA;Initial Catalog=Test;Integrated Security=True;Encrypt=True;Trust Server Certificate=True")
                .Options;

            Context = new MyShopContext(options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            Context.ChangeTracker.Clear();

        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
