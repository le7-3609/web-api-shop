using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.IntegretionTests
{
    public class SubCategoryRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly MyShopContext _context;
        private readonly SubCategoryRepository _repository;

        public SubCategoryRepositoryIntegrationTests(DatabaseFixture fixture)
        {
            _context = fixture.Context;
            _context.ChangeTracker.Clear();

            // Clear hierarchy
            _context.Products.RemoveRange(_context.Products);
            _context.SubCategories.RemoveRange(_context.SubCategories);
            _context.MainCategories.RemoveRange(_context.MainCategories);
            _context.SaveChanges();

            _repository = new SubCategoryRepository(_context);
        }

        private async Task<MainCategory> SeedMainCategoryAsync()
        {
            var main = new MainCategory { MainCategoryName = "Hardware", MainCategoryPrompt = "P" };
            await _context.MainCategories.AddAsync(main);
            await _context.SaveChangesAsync();
            return main;
        }

        #region Happy Paths

        [Fact]
        public async Task GetSubCategoryAsync_WithDescriptionFilter_ReturnsMatches()
        {
            // Arrange
            var main = await SeedMainCategoryAsync();
            var sub = new SubCategory
            {
                SubCategoryName = "Laptops",
                CategoryDescription = "Portable Computers",
                SubCategoryPrompt = "P",
                MainCategoryId = main.MainCategoryId
            };
            await _context.SubCategories.AddAsync(sub);
            await _context.SaveChangesAsync();

            // Act
            var (results, total) = await _repository.GetSubCategoryAsync(1, 10, "Portable", new int?[] { main.MainCategoryId });

            // Assert
            Assert.Equal(1, total);
            Assert.Equal("Laptops", results.First().SubCategoryName);
        }

        [Fact]
        public async Task UpdateSubCategoryAsync_ShouldPersistChanges()
        {
            // Arrange
            var main = await SeedMainCategoryAsync();
            var sub = new SubCategory { SubCategoryName = "Old Name", SubCategoryPrompt = "P", MainCategoryId = main.MainCategoryId };
            await _context.SubCategories.AddAsync(sub);
            await _context.SaveChangesAsync();
            _context.Entry(sub).State = EntityState.Detached;

            // Act
            sub.SubCategoryName = "New Name";
            await _repository.UpdateSubCategoryAsync(sub.SubCategoryId, sub);

            // Assert
            var updated = await _context.SubCategories.FindAsync(sub.SubCategoryId);
            Assert.Equal("New Name", updated.SubCategoryName);
        }

        #endregion

        #region Unhappy Paths

        [Fact]
        public async Task DeleteSubCategoryAsync_WithLinkedProducts_ReturnsFalse()
        {
            // Arrange
            var main = await SeedMainCategoryAsync();
            var sub = new SubCategory { SubCategoryName = "ProtectMe", SubCategoryPrompt = "P", MainCategoryId = main.MainCategoryId };
            await _context.SubCategories.AddAsync(sub);
            await _context.SaveChangesAsync();

            var product = new Product { ProductName = "Locked Item", ProductPrompt = "P", SubCategoryId = sub.SubCategoryId };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteSubCategoryAsync(sub.SubCategoryId);

            // Assert
            Assert.False(result); // Cannot delete due to product FK
        }

        #endregion
    }
}