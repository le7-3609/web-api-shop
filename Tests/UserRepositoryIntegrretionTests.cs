using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UserRepositoryIntegrationTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly MyShopContext _context;
        private readonly UserRepository _repository;

        public UserRepositoryIntegrationTests(DatabaseFixture fixture)
        {
            _context = fixture.Context;

            _context.Database.CloseConnection();
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _repository = new UserRepository(_context);
        }

 
        #region Happy Paths

        [Fact]
        public async Task RegisterAsync_ShouldSaveUserToRealDatabase()
        {
            // Arrange
            var user = new User { Email = "integration@test.com", Password = "123", FirstName = "Test", LastName = "User", Phone = "0" };

            // Act
            var result = await _repository.RegisterAsync(user);

            // Assert
            var userInDb = await _context.Users.FindAsync(result.UserId);
            Assert.NotNull(userInDb);
            Assert.Equal("integration@test.com", userInDb.Email);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsUserFromDb()
        {
            // Arrange
            var user = new User { Email = "login@integration.com", Password = "password123", FirstName = "A", LastName = "B", Phone = "0" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.LoginAsync("login@integration.com", "password123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("login@integration.com", result.Email);
        }

        #endregion

        #region Unhappy Paths

        [Fact]
        public async Task GetByIdAsync_WhenUserDoesNotExist_ReturnsNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUserSuccessfully()
        {
            // Arrange
            var user = new User { Email = "old@test.com", Password = "123", FirstName = "Old", LastName = "Name", Phone = "0" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            _context.Entry(user).State = EntityState.Detached;

            // Act
            user.FirstName = "NewName";
            var result = await _repository.UpdateAsync(user);

            // Assert
            var updatedUser = await _context.Users.FindAsync(user.UserId);
            Assert.Equal("NewName", updatedUser.FirstName);
        }

        #endregion

        public void Dispose()
        {
            _context.Database.CloseConnection();
            _context.Database.EnsureDeleted();
        }
    }
}