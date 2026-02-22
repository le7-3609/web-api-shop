using Entities;
using Repositories;
using Xunit;

namespace Tests.IntegrationTests
{
    [Collection("Database collection")]
    public class PlatformRepositoryIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly MyShopContext _context;
        private readonly PlatformRepository _repository;

        public PlatformRepositoryIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _context = fixture.Context;
            _repository = new PlatformRepository(_context);
            _fixture.ClearDatabase();
        }

        [Fact]
        public async Task AddPlatformAsync_AddsPlatform()
        {
            var platform = new Platform { PlatformName = "NewPlat" };
            var saved = await _repository.AddPlatformAsync(platform);
            Assert.True(saved.PlatformId > 0);
        }

        [Fact]
        public async Task GetPlatformByIdAsync_ReturnsPlatform()
        {
            var p = new Platform { PlatformId = 77, PlatformName = "P77" };
            _context.Platforms.Add(p);
            _context.SaveChanges();

            var got = await _repository.GetPlatformByIdAsync(77);
            Assert.NotNull(got);
            Assert.Equal("P77", got.PlatformName);
        }
    }
}
